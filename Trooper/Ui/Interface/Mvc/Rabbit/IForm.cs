namespace Trooper.Ui.Interface.Mvc.Rabbit
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web;
    using Trooper.Ui.Mvc.Rabbit.Controls;
    using Trooper.Ui.Mvc.Rabbit.Models;

    public interface IForm<TModel>
    {
        IHtml Html { get; }

        IHtmlString BeginForm();

        IHtmlString Button(Button bProps);

        IHtmlString ButtonFor<TValue>(Expression<Func<TModel, TValue>> expression, Button bProps);

        IHtmlString CheckBox(CheckBox cbProps);

        IHtmlString CheckBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, CheckBox cbProps);

        IHtmlString CheckBoxList<TOptionKey, TOptionValue>(CheckBoxList<TOptionKey, TOptionValue> cblProps);

        IHtmlString CheckBoxListFor<TOptionKey, TOptionValue>(Expression<Func<TModel, IList<TOptionKey>>> expression, CheckBoxList<TOptionKey, TOptionValue> cblProps);               

        IHtmlString CurrencyBox(DecimalBox cProps);

        IHtmlString CurrencyBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, DecimalBox dProps);

        IHtmlString DateTimePicker(DateTimePicker dtpProps);

        IHtmlString DateTimePickerFor<TValue>(Expression<Func<TModel, TValue>> expression, DateTimePicker dtpProps);

        IHtmlString DecimalBox(DecimalBox dProps);

        IHtmlString DecimalBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, DecimalBox dProps);

        IHtmlString EndForm();
        
        IHtmlString IntegerBox(IntegerBox iProps);

        IHtmlString IntegerBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, IntegerBox iProps);

        IHtmlString MultiSelectListFor<TOptionKey, TOptionValue>(Expression<Func<TModel, IList<TOptionKey>>> expression, SelectList<TOptionKey, TOptionValue> sProps);

        IHtmlString PercentageBox(DecimalBox pProps);

        IHtmlString PercentageBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, DecimalBox dProps);

        IHtmlString RadioList<TOptionKey, TOptionValue>(RadioList<TOptionKey, TOptionValue> rlProps);

        IHtmlString RadioListFor<TOptionKey, TOptionValue>(Expression<Func<TModel, TOptionKey>> expression, RadioList<TOptionKey, TOptionValue> rlProps);

        IHtmlString SearchBox(SearchBox sbProps);

        IHtmlString SearchBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, SearchBox sbProps);

        IHtmlString SelectList<TOption>(SelectList<TOption> sProps);

        IHtmlString SelectList<TOptionKey, TOptionValue>(SelectList<TOptionKey, TOptionValue> sProps);

        IHtmlString SelectListFor<TOption>(Expression<Func<TModel, TOption>> expression, SelectList<TOption> sProps);

        IHtmlString SelectListFor<TOptionKey, TOptionValue>(Expression<Func<TModel, TOptionKey>> expression, SelectList<TOptionKey, TOptionValue> sProps);               

        IHtmlString Table<T>(TableControl<T> tProps) where T : class;

        IHtmlString TableFor<T>(Expression<Func<TModel, TableModel>> expression, TableControl<T> tProps) where T : class;

        IHtmlString TextareaBox(TextareaBox tabProps);

        IHtmlString TextareaBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, TextareaBox tabProp);

        IHtmlString TextBox(TextBox tbProps);

        IHtmlString TextBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, TextBox tbProps);

        IHtmlString Upload(UploadBox ubProps);

        IHtmlString UploadFor(Expression<Func<TModel, UploadModel>> expression, UploadBox ubProps);
    }
}
