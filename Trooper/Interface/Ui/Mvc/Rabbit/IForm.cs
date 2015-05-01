using Trooper.Ui.Mvc.Rabbit.Props;

namespace Trooper.Ui.Interface.Mvc.Rabbit
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web;
    using Trooper.Ui.Mvc.Rabbit.Models.Table;
    using Ui.Mvc.Rabbit.Models;

    public interface IForm<TModel>
    {
        IHtml Html { get; }

        IHtmlString BeginForm();

        IHtmlString Button(ButtonProps bProps);

        IHtmlString ButtonFor<TValue>(Expression<Func<TModel, TValue>> expression, ButtonProps bProps);

        IHtmlString CheckBox(CheckBoxProps cbProps);

        IHtmlString CheckBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, CheckBoxProps cbProps);

        IHtmlString CheckBoxList<TOptionKey, TOptionValue>(CheckBoxListProps<TOptionKey, TOptionValue> cblProps);

        IHtmlString CheckBoxListFor<TOptionKey, TOptionValue>(Expression<Func<TModel, IList<TOptionKey>>> expression, CheckBoxListProps<TOptionKey, TOptionValue> cblProps);               

        IHtmlString CurrencyBox(DecimalBoxProps cProps);

        IHtmlString CurrencyBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, DecimalBoxProps dProps);

        IHtmlString DateTimePicker(DateTimePickerProps dtpProps);

        IHtmlString DateTimePickerFor<TValue>(Expression<Func<TModel, TValue>> expression, DateTimePickerProps dtpProps);

        IHtmlString DecimalBox(DecimalBoxProps dProps);

        IHtmlString DecimalBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, DecimalBoxProps dProps);

        IHtmlString EndForm();
        
        IHtmlString IntegerBox(IntegerBoxProps iProps);

        IHtmlString IntegerBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, IntegerBoxProps iProps);

		IHtmlString MultiSelectListFor<TOptionKey, TOptionValue>(Expression<Func<TModel, IList<TOptionKey>>> expression, SelectListProps<TOptionKey, TOptionValue> sProps);

        IHtmlString PercentageBox(DecimalBoxProps pProps);

        IHtmlString PercentageBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, DecimalBoxProps dProps);

        IHtmlString RadioList<TOptionKey, TOptionValue>(RadioListProps<TOptionKey, TOptionValue> rlProps);

        IHtmlString RadioListFor<TOptionKey, TOptionValue>(Expression<Func<TModel, TOptionKey>> expression, RadioListProps<TOptionKey, TOptionValue> rlProps);

        IHtmlString SearchBox(SearchBoxProps sbProps);

        IHtmlString SearchBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, SearchBoxProps sbProps);

        IHtmlString SelectList<TOption>(SelectListProps<TOption> sProps);

		IHtmlString SelectList<TOptionKey, TOptionValue>(SelectListProps<TOptionKey, TOptionValue> sProps);

        IHtmlString SelectListFor<TOption>(Expression<Func<TModel, TOption>> expression, SelectListProps<TOption> sProps);

		IHtmlString SelectListFor<TOptionKey, TOptionValue>(Expression<Func<TModel, TOptionKey>> expression, SelectListProps<TOptionKey, TOptionValue> sProps);               

        IHtmlString Table<T>(TableProps<T> tProps) where T : class;

        IHtmlString TableFor<T>(Expression<Func<TModel, TableModel<T>>> expression, TableProps<T> tProps) where T : class;

        IHtmlString TextareaBox(TextareaBoxProps tabProps);

        IHtmlString TextareaBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, TextareaBoxProps tabProp);

        IHtmlString TextBox(TextBoxProps tbProps);

        IHtmlString TextBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, TextBoxProps tbProps);

        IHtmlString Upload(UploadBoxProps ubProps);

        IHtmlString UploadFor(Expression<Func<TModel, UploadModel>> expression, UploadBoxProps ubProps);
    }
}
