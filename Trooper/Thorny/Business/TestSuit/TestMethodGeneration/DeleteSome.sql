select 
	Entries.[Entry], Entries.State, Entries.itemState, Entries.identityState, Entries.Result,
	'[Test]public virtual void ' + Entries.[Entry] + '() {}'
from
	(select 
		s.State, Pair.itemState, Pair.identityState, o.Result, Pair.Fault, o.Pass, Pair.NeedsItems,
		s.State+'_Items'+Pair.itemState+'_Identity'+Pair.identityState+'_'+o.Result as [Entry]
	from 
		System s
		cross join 
			(select 
				itemParam.State as itemState, 
				identityParam.State identityState, 
				identityParam.Fault identityFault,
				itemParam.NeedsItems as NeedsItems,
				case when itemParam.Fault = 1 or identityParam.Fault = 1 then 1 else Null end as Fault
			from 
				Parameter itemParam
				cross join Parameter identityParam
			where 
				itemParam.Action = 'DeletingSome'
				and identityParam.Action = 'All') as Pair
		cross join 
			(select 
				o.Result, o.Pass, o.NeedsItems
			from 
				Outcome o
			where 
				o.Action in ('DeletingSome', 'All')) as o
	where
		not (IsNull(Pair.NeedsItems, 0) = 1 and s.State = 'IsEmpty')
		and not (IsNull(Pair.Fault, 0) = 1 and IsNull(o.Pass, 0) = 1)
	) as Entries
where
	[Entry] not in (
		'HasItems_ItemsAllInvalidAllExist_IdentityIsAllowed_ReportsErrorAndNoChange',
		'HasItems_ItemsAllValidAllExist_IdentityIsAllowed_ReportsErrorAndNoChange',
		'HasItems_ItemsSomeValidAllExist_IdentityIsAllowed_ReportsErrorAndNoChange'
	)
order by [Entry]

	
