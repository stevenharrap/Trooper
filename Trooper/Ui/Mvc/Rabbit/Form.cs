//--------------------------------------------------------------------------------------
// <copyright file="Form.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards..
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Rabbit
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Web;
	using System.Web.Helpers;
	using System.Web.Mvc;
	using System.Web.Mvc.Html;
	using BusinessOperation2;
	using BusinessOperation2.Interface.OperationResponse;
	using BusinessOperation2.Utility;
	using Properties;
	using Interface.Mvc.Cruncher;
	using Controls;
	using Models;
	using Trooper.Utility;
    using Trooper.Ui.Interface.Mvc.Rabbit;
    using Trooper.Ui.Mvc.Utility;

	/// <summary>
    /// Bootstrap is a CSS library from Twitter. It is very good at Html5 layout and provides
    /// flexible device independent display. This class provides a list of form controls with
    /// extra help from Bootstrap and JQuery for a better user experience. Using this class 
    /// in your pages with Cruncher will automatically inject the Jquery and Bootstrap client
    /// side requirements.
    /// <see cref="http://twitter.github.io/bootstrap/"/>
    /// </summary>
    /// <typeparam name="TModel">
    /// The model type in your view
    /// </typeparam>
    public class Form<TModel> : IForm<TModel>
    {
        public IHtml Html { get; private set; }

        private IGoRabbit<TModel> goRabbit;

        public FormControl FormProps { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Form{TModel}"/> class.
		/// By instantiating this the Html class will inject the JQuery and Bootstrap JS and CSS classes.
		/// </summary>
		/// <param name="htmlHelper">
		/// The html helper.
		/// </param>
		/// <param name="fhProps"></param>
        public Form(FormControl fhProps, IGoRabbit<TModel> goRabbit, IHtml html)
        {
            this.goRabbit = goRabbit;
            this.FormProps = fhProps;
            this.Html = html;

            this.Html.RegisterControl(this.FormProps);

            this.Init();
        }

        #region public properties        

        #endregion

        #region public methods

        #region form

        public IHtmlString BeginForm()
        {
            var tag = string.Format(
                "<form id=\"{0}\" action=\"{1}\" method=\"{2}\">", 
                this.FormProps.Id, 
                this.FormProps.Action, 
                this.FormProps.Method);

            return new MvcHtmlString(tag);
        }

        public IHtmlString EndForm()
        {
            return new MvcHtmlString("</form>");
        }

        #endregion

        #region controls

        public IHtmlString TextBox(TextBox tbProps)
        {
            this.Html.RegisterControl(tbProps);

            var result = this.MakeTextBox(tbProps, true);

            result = this.MakeFormGroup(result, tbProps);

            return MvcHtmlString.Create(result);
        }

        public IHtmlString TextBoxFor<TValue>(
            Expression<Func<TModel, TValue>> expression, 
            TextBox tbProps)
        {
            var value = Conversion.ConvertToString(RabbitHelper.GetExpressionValue<TModel, TValue>(expression, this.goRabbit.HtmlHelper));

	        tbProps.Name = this.goRabbit.HtmlHelper.NameFor(expression).ToString();
            tbProps.Id = this.GetIdFromName(tbProps);
            tbProps.Messages = this.GetMessagesForProperty(expression, this.FormProps.Messages);
            tbProps.Value = value;

            return this.TextBox(tbProps);
        }

        public IHtmlString IntegerBox(IntegerBox iProps)
        {
            this.Html.RegisterControl(iProps);

            var tbProps = iProps as TextBox;

            tbProps.Value = iProps.Value == null ? string.Empty : iProps.Value.ToString();

            var result = this.MakeTextBox(tbProps, false);

            result = this.MakeFormGroup(result, iProps);

            this.IncludeNumericJs();

            var js = string.Format(
                "new trooper.ui.control.numericBox({{id:'{0}', formId:'{1}', numericType:'Integer', minimum:{2}, maximum:{3}, decimalDigits:0}});",
                iProps.Id,
                this.FormProps.Id,
                iProps.Minimum == null ? "null" : Conversion.ConvertToString(iProps.Minimum),
                iProps.Maximum == null ? "null" : Conversion.ConvertToString(iProps.Maximum));

            this.goRabbit.Cruncher.AddJsInline(js, OrderOptions.Last);

            return MvcHtmlString.Create(result);
        }

        public IHtmlString IntegerBoxFor<TValue>(
            Expression<Func<TModel, TValue>> expression,
            IntegerBox iProps)
        {
            var value = Conversion.ConvertToInt(RabbitHelper.GetExpressionValue<TModel, TValue>(expression, this.goRabbit.HtmlHelper));

			iProps.Name = this.goRabbit.HtmlHelper.NameFor(expression).ToString();
            iProps.Id = this.GetIdFromName(iProps);
            iProps.Messages = this.GetMessagesForProperty(expression, this.FormProps.Messages);
            iProps.Value = value;

            return this.IntegerBox(iProps);
        }

        public IHtmlString DecimalBox(DecimalBox dProps)
        {
            this.Html.RegisterControl(dProps);

            var tbProps = dProps as TextBox;

            tbProps.Value = this.FormatDecimal(dProps.Value, dProps.DecimalDigits);

            var output = this.MakeTextBox(tbProps, false);

            output = this.MakeFormGroup(output, dProps);
            
            this.IncludeNumericJs();

            var js = string.Format(
                "new trooper.ui.control.numericBox({{id:'{0}', formId:'{1}', numericType:'Decimal', minimum:{2}, maximum:{3}, decimalDigits:{4}}});",
                dProps.Id,
                this.FormProps.Id,
                dProps.Minimum == null ? "null" : Conversion.ConvertToString(dProps.Minimum),
                dProps.Maximum == null ? "null" : Conversion.ConvertToString(dProps.Maximum),
				dProps.DecimalDigits == null ? "null" : Conversion.ConvertToString(dProps.DecimalDigits));

            this.goRabbit.Cruncher.AddJsInline(js, OrderOptions.Last);

            return MvcHtmlString.Create(output);
        }

        public IHtmlString DecimalBoxFor<TValue>(
            Expression<Func<TModel, TValue>> expression,
            DecimalBox dProps)
        {
            var value = Conversion.ConvertToDecimal(RabbitHelper.GetExpressionValue<TModel, TValue>(expression, this.goRabbit.HtmlHelper));

			dProps.Name = this.goRabbit.HtmlHelper.NameFor(expression).ToString();
            dProps.Id = this.GetIdFromName(dProps);
            dProps.Messages = this.GetMessagesForProperty(expression, this.FormProps.Messages);
            dProps.Value = value;

            return this.DecimalBox(dProps);
        }

        public IHtmlString PercentageBox(DecimalBox pProps)
        {
            this.Html.RegisterControl(pProps);
            
            if (pProps.DecimalDigits == null)
            {
                pProps.DecimalDigits = 2;
            }
            
            var tbProps = pProps as TextBox;

            tbProps.Value = this.FormatDecimal(pProps.Value, pProps.DecimalDigits);

            var output = this.MakeTextBox(tbProps, false);

            output = this.MakeInputGroup(output + "<span class=\"input-group-addon\">%</span>\n");

            output = this.MakeFormGroup(output, pProps);

            this.IncludeNumericJs();

            var js = string.Format(
                "new trooper.ui.control.numericBox({{id:'{0}', formId:'{1}', numericType:'Percentage', minimum:{2}, maximum:{3}, decimalDigits:{4}}});",
                pProps.Id,
                this.FormProps.Id,
                pProps.Minimum == null ? "null" : Conversion.ConvertToString(pProps.Minimum),
                pProps.Maximum == null ? "null" : Conversion.ConvertToString(pProps.Maximum),
                pProps.DecimalDigits);

            this.goRabbit.Cruncher.AddJsInline(js, OrderOptions.Last);

            return MvcHtmlString.Create(output);
        }

        public IHtmlString PercentageBoxFor<TValue>(
            Expression<Func<TModel, TValue>> expression,
            DecimalBox dProps)
        {
            var value = Conversion.ConvertToDecimal(RabbitHelper.GetExpressionValue<TModel, TValue>(expression, this.goRabbit.HtmlHelper));

			dProps.Name = this.goRabbit.HtmlHelper.NameFor(expression).ToString();
            dProps.Id = this.GetIdFromName(dProps);
            dProps.Messages = this.GetMessagesForProperty(expression, this.Html.Messages);
            dProps.Value = value;

            return this.PercentageBox(dProps);
        }

        public IHtmlString CurrencyBox(DecimalBox cProps)
        {
            this.Html.RegisterControl(cProps);

            if (cProps.DecimalDigits == null)
            {
                cProps.DecimalDigits = 2;
            }

            var tbProps = cProps as TextBox;

            tbProps.Value = this.FormatDecimal(cProps.Value, cProps.DecimalDigits);

            var output = this.MakeTextBox(tbProps, false);

            output = this.MakeInputGroup("<span class=\"input-group-addon\">$</span>\n" + output);

            output = this.MakeFormGroup(output, cProps);

            this.IncludeNumericJs();

            var js = string.Format(
                "new trooper.ui.control.numericBox({{id:'{0}', formId:'{1}', numericType:'Currency', minimum:{2}, maximum:{3}, decimalDigits:{4}}});",
                cProps.Id,
                this.FormProps.Id,
                cProps.Minimum == null ? "null" : Conversion.ConvertToString(cProps.Minimum),
                cProps.Maximum == null ? "null" : Conversion.ConvertToString(cProps.Maximum),
                cProps.DecimalDigits ?? 0);

            this.goRabbit.Cruncher.AddJsInline(js, OrderOptions.Last);

            return MvcHtmlString.Create(output);
        }

        public IHtmlString CurrencyBoxFor<TValue>(
            Expression<Func<TModel, TValue>> expression,
            DecimalBox dProps)
        {
            var value = Conversion.ConvertToDecimal(RabbitHelper.GetExpressionValue<TModel, TValue>(expression, this.goRabbit.HtmlHelper));

			dProps.Name = this.goRabbit.HtmlHelper.NameFor(expression).ToString();
            dProps.Id = this.GetIdFromName(dProps);
            dProps.Messages = this.GetMessagesForProperty(expression, this.FormProps.Messages);
            dProps.Value = value;

            return this.CurrencyBox(dProps);
        }

        public IHtmlString TextareaBox(TextareaBox tabProps)
        {
            this.Html.RegisterControl(tabProps);

            if (!this.goRabbit.Cruncher.HasJsItem("textareaBox_js"))
            {
                this.goRabbit.Cruncher.AddJsInline(Resources.textareaBox_js, "textareaBox_js", OrderOptions.Middle);
            }

            var js = string.Format(
                "new trooper.ui.control.textareaBox({{id:'{0}', formId:'{1}', maxLength:{2}, warnOnLeave:{3}}});",
                tabProps.Id,
                this.FormProps.Id,
                Conversion.ConvertToInt(tabProps.MaxLength, 0),
                RabbitHelper.GetJsBool(tabProps.WarnOnLeave));

            this.goRabbit.Cruncher.AddJsInline(js, OrderOptions.Last);            

            var textBoxAttrs = new Dictionary<string, object>();

            var cssClasses = RabbitHelper.AddClass(null, "form-control");

            if (!this.IsControlEnabled(tabProps.Enabled))
            {
                textBoxAttrs.Add("readonly", "readonly");
            }

            if (tabProps.MaxLength != null && tabProps.MaxLength > 0)
            {
                textBoxAttrs.Add("maxlength", tabProps.MaxLength);
            }

            if (tabProps.Rows != null && tabProps.Rows > 0)
            {
                textBoxAttrs.Add("rows", tabProps.Rows);
            }

            //// Lets make the control
            var result = string.Format(
                "<textarea id=\"{0}\" name=\"{1}\" {3} {4} />{2}</textarea>\n",
                tabProps.Id,
                tabProps.Name,
                HttpUtility.HtmlAttributeEncode(tabProps.Value),
                RabbitHelper.MakeClassAttribute(cssClasses),
                textBoxAttrs.Aggregate(
                    string.Empty,
                    (current, next) =>
                    current
                    + string.Format(
                        " {0}=\"{1}\"", next.Key, HttpUtility.HtmlAttributeEncode(next.Value.ToString()))));

            result = this.MakeFormGroup(result, tabProps);

            return MvcHtmlString.Create(result);
        }

        public IHtmlString TextareaBoxFor<TValue>(
            Expression<Func<TModel, TValue>> expression,
            TextareaBox tabProp)
        {
            var value = Conversion.ConvertToString(RabbitHelper.GetExpressionValue<TModel, TValue>(expression, this.goRabbit.HtmlHelper));

			tabProp.Name = this.goRabbit.HtmlHelper.NameFor(expression).ToString();
            tabProp.Id = this.GetIdFromName(tabProp);
            tabProp.Messages = this.GetMessagesForProperty(expression, this.FormProps.Messages);
            tabProp.Value = value;

            return this.TextareaBox(tabProp);
        }

        public IHtmlString Button(Button bProps)
        {
            this.Html.RegisterControl(bProps);

            if (!this.goRabbit.Cruncher.HasJsItem("button_js"))
            {
                this.goRabbit.Cruncher.AddJsInline(Resources.button_js, "button_js", OrderOptions.Middle);
            }

            var js = string.Format(
                "new trooper.ui.control.button({{id:'{0}', "
                + "formId:'{1}', "
                + "url:'{2}', "
                + "targetNewWindow:{3}, "
                + "launchLoadingOnClick:{4}, "
                + "loadingScreenTitle:'{5}', "
                + "confirmOnClick:{6}, "
                + "confirmTitle:'{7}', "
                + "submit: {8} }});",
                bProps.Id,
                this.FormProps.Id,
                bProps.Url ?? string.Empty,
                RabbitHelper.GetJsBool(bProps.TargetNewWindow),
                RabbitHelper.GetJsBool(bProps.LaunchLoadingOnclick),
                bProps.LoadingScreenTitle ?? string.Empty,
                RabbitHelper.GetJsBool(bProps.ConfirmOnClick),
                bProps.ConfirmTitle ?? string.Empty,
                RabbitHelper.GetJsBool(bProps.Submit));

            this.goRabbit.Cruncher.AddJsInline(js, OrderOptions.Last);

            var buttonClasses = RabbitHelper.AddClasses(null, bProps.ButtonClasses);
            RabbitHelper.AddClass(buttonClasses, RabbitHelper.ButtonTypeToString(bProps.ButtonType));
            RabbitHelper.AddClass(buttonClasses, "btn");
            RabbitHelper.AddClass(buttonClasses, bProps.Visible ? string.Empty : "hidden");

            var attrs = RabbitHelper.AddAttributes(null, bProps.Attrs);
            RabbitHelper.AddAttribute(attrs, "type", bProps.Submit ? "submit" : "button");
            RabbitHelper.AddAttribute(attrs, "id", bProps.Id);
            RabbitHelper.AddAttribute(attrs, "name", bProps.Name);
            RabbitHelper.AddAttribute(attrs, "value", bProps.Value);
            RabbitHelper.AddNotEmptyAttribute(attrs, "disabled", this.IsControlEnabled(bProps.Enabled) ? string.Empty : "disabled");
            RabbitHelper.AddNotEmptyAttribute(attrs, "title", bProps.ToolTip);
            RabbitHelper.AddAttribute(attrs, "class", RabbitHelper.MakeClassAttributeContent(buttonClasses));

            var output = string.Format(
                "<button {0}>{1}{2}</a>",
                RabbitHelper.MakeAttributesList(attrs),
                string.IsNullOrEmpty(bProps.Icon)
                    ? string.Empty
                    : string.Format("{0} ", RabbitHelper.MakeIcon(bProps.Icon)),
                bProps.Title);

            return MvcHtmlString.Create(output);
        }

        public IHtmlString ButtonFor<TValue>(Expression<Func<TModel, TValue>> expression, Button bProps)
        {
            var value = Conversion.ConvertToString(RabbitHelper.GetExpressionValue<TModel, TValue>(expression, this.goRabbit.HtmlHelper));

			bProps.Name = this.goRabbit.HtmlHelper.NameFor(expression).ToString();
            bProps.Id = this.GetIdFromName(bProps);
            bProps.Messages = this.GetMessagesForProperty(expression, this.FormProps.Messages);
            bProps.Value = value;

            return this.Button(bProps);
        }

        public IHtmlString Upload(UploadBox ubProps)
        {
            this.Html.RegisterControl(ubProps);

            if (ubProps.UploadModel == null)
            {
                return MvcHtmlString.Create(string.Empty);
            }

            var action = this.goRabbit.UrlHelper.Action("OpenIframe", "RabbitUpload", new { id = ubProps.Id });

            var output = string.Format("<div class=\"trooper upload\" id=\"{0}_container\">", ubProps.Id);

            output += "<iframe style=\"display:none\" src=\"" + action + "\"></iframe>";

            var persistData =
                new
                    {
                        ubProps.UploadModel.CurrentDocumentFilename,
                        ubProps.UploadModel.CurrentDocumentDeleted,
                        ubProps.UploadModel.CurrentDocumentUrl,
                        ubProps.UploadModel.PersistedId,
                        ubProps.UploadModel.PersistedFilename
                    };

            output +=
                string.Format(
                    "<textarea style=\"display:none\" name=\"{0}.PersistedData\" id=\"{1}\"/>{2}</textarea>",
                    ubProps.Name,
                    ubProps.Id,
                    Json.Encode(persistData));

            string level;

            switch (this.GetControlAlertLevel(ubProps))
            {
                case MessageAlertLevel.Error:
                    level = "danger";
                    break;
                case MessageAlertLevel.Warning:
                    level = "warning";
                    break;
                default:
                    level = "default";
                    break;
            }

            output += "<div class=\"input-group\">"
                      + "<input type=\"text\" class=\"form-control display\" readonly=\"readonly\" />"
                      + "<span class=\"input-group-btn\">"
                      + "<button type=\"button\" class=\"clear btn btn-" + level + "\" ><i class=\"glyphicon glyphicon-remove-circle\"></i></button>"
                      + "<button type=\"button\" class=\"browse btn btn-" + level + "\"><i class=\"glyphicon glyphicon-list-alt\"></i></button>"
                      + "</span></div></div>\n";

            output = this.MakeFormGroup(output, ubProps);

            if (!this.goRabbit.Cruncher.HasJsItem("upload_js"))
            {
                this.goRabbit.Cruncher.AddJsInline(Resources.upload_js, "upload_js", OrderOptions.Middle);

                this.goRabbit.Cruncher.AddLessInline(Resources.upload_less, "upload_less", OrderOptions.Middle);
            }

            var js = string.Format(
                "new trooper.ui.control.upload({{id:'{0}', formId:'{1}', warnOnLeave: {2}}});",
                ubProps.Id,
                this.FormProps.Id,
                RabbitHelper.GetJsBool(ubProps.WarnOnLeave));

            this.goRabbit.Cruncher.AddJsInline(js, OrderOptions.Last);

            return MvcHtmlString.Create(output);
        }

        public IHtmlString UploadFor(Expression<Func<TModel, UploadModel>> expression, UploadBox ubProps)
        {
            var value = RabbitHelper.GetExpressionValue<TModel, UploadModel>(expression, this.goRabbit.HtmlHelper);

			ubProps.Name = this.goRabbit.HtmlHelper.NameFor(expression).ToString();
            ubProps.Id = this.GetIdFromName(ubProps);
            ubProps.Messages = this.GetMessagesForProperty(expression, this.FormProps.Messages);
            ubProps.UploadModel = value;

            return this.Upload(ubProps);
        }

        public IHtmlString CheckBoxList<TOptionKey, TOptionValue>(CheckBoxList<TOptionKey, TOptionValue> cblProps)
        {
            this.Html.RegisterControl(cblProps);

            var output = cblProps.ScrollHeight > 0
                ? string.Format("<div id=\"{0}\" class=\"trooper y-scrolling\" style=\"height: {1}px\">", cblProps.Id, cblProps.ScrollHeight)
                : string.Format("<div id=\"{0}\">", cblProps.Id);
            
            if (cblProps.Options != null)
            {
                foreach (var item in cblProps.Options)
                {
                    var inpAttrs = RabbitHelper.AddAttribute(null, "type", "checkbox");
                    RabbitHelper.AddAttribute(inpAttrs, "name", cblProps.Name);
                    RabbitHelper.AddAttribute(inpAttrs, "value", item.Key.ToString());
                    RabbitHelper.AddNotEmptyAttribute(
                        inpAttrs,
                        "checked",
                        item.Value != null && cblProps.SelectedOptions != null && cblProps.SelectedOptions.Contains(item.Key) ? "checked" : null);
                    RabbitHelper.AddNotEmptyAttribute(
                        inpAttrs,
                        "disabled",
                        this.IsControlEnabled(cblProps.Enabled) ? null : "disabled");
                    
                    output += string.Format(
                        "<div class=\"checkbox\"><label class=\"{0}\"><input {1} />{2}</label></div>\n",
                        cblProps.Inline ? "checkbox-inline" : string.Empty, 
                        RabbitHelper.MakeAttributesList(inpAttrs), 
                        item.Value);
                }
            }
            
            output += "</div>";

            output = this.MakeFormGroup(output, cblProps);

            if (!this.goRabbit.Cruncher.HasJsItem("checkBoxList_js"))
            {
                this.goRabbit.Cruncher.AddJsInline(Resources.checkBoxList_js, "checkBoxList_js", OrderOptions.Middle);
            }

            var js = string.Format(
                "new trooper.ui.control.checkBoxList({{id:'{0}', formId:'{1}', name:'{2}', warnOnLeave:{3} }});",
                cblProps.Id,
                this.FormProps.Id,
                cblProps.Name,
                RabbitHelper.GetJsBool(cblProps.WarnOnLeave));

            this.goRabbit.Cruncher.AddJsInline(js, OrderOptions.Middle);

            return MvcHtmlString.Create(output);
        }

        public IHtmlString CheckBoxListFor<TOptionKey, TOptionValue>(
            Expression<Func<TModel, IList<TOptionKey>>> expression,
            CheckBoxList<TOptionKey, TOptionValue> cblProps)
        {
            var value = RabbitHelper.GetExpressionValue<TModel, IList<TOptionKey>>(expression, this.goRabbit.HtmlHelper);

			cblProps.Name = this.goRabbit.HtmlHelper.NameFor(expression).ToString();
            cblProps.Id = this.GetIdFromName(cblProps);
            cblProps.Messages = this.GetMessagesForProperty(expression, this.FormProps.Messages);
            cblProps.SelectedOptions = value;

            return this.CheckBoxList(cblProps);
        }

        public IHtmlString CheckBox(CheckBox cbProps)
        {
            this.Html.RegisterControl(cbProps);

            var inpAttrs = RabbitHelper.AddAttribute(null, "type", "checkbox");
            RabbitHelper.AddAttribute(inpAttrs, "name", cbProps.Name);
            RabbitHelper.AddAttribute(inpAttrs, "value", cbProps.Value);
            RabbitHelper.AddNotEmptyAttribute(inpAttrs, "checked", (cbProps.Checked ?? false) ? "checked" : null);
            RabbitHelper.AddNotEmptyAttribute(inpAttrs, "disabled", this.IsControlEnabled(cbProps.Enabled) ? null : "disabled");

            var output = string.Format(
                "<div class=\"checkbox\"><label class=\"{0}\"><input {1} />{2}</label></div>\n",
                cbProps.Inline ? "checkbox-inline" : string.Empty,
                RabbitHelper.MakeAttributesList(inpAttrs),
                cbProps.Title);

            if (!this.goRabbit.Cruncher.HasJsItem("checkBox_js"))
            {
                this.goRabbit.Cruncher.AddJsInline(Resources.checkBox_js, "checkBox_js", OrderOptions.Middle);
            }

            var js = string.Format(
                "new trooper.ui.control.checkBox({{id:'{0}', formId:'{1}', warnOnLeave:{2}}});",
                cbProps.Id,
                this.FormProps.Id,
                RabbitHelper.GetJsBool(cbProps.WarnOnLeave));

            this.goRabbit.Cruncher.AddJsInline(js, OrderOptions.Last);

            return MvcHtmlString.Create(output);
        }

        public IHtmlString CheckBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, CheckBox cbProps)
        {
            var value = Conversion.ConvertToBoolean(RabbitHelper.GetExpressionValue<TModel, TValue>(expression, this.goRabbit.HtmlHelper));

			cbProps.Name = this.goRabbit.HtmlHelper.NameFor(expression).ToString();
            cbProps.Id = this.GetIdFromName(cbProps);
            cbProps.Messages = this.GetMessagesForProperty(expression, this.FormProps.Messages);
            cbProps.Checked = value;

            return this.CheckBox(cbProps);
        }


        public IHtmlString SelectList<TOption>(
			SelectList<TOption> sProps)
	    {
		    return this.SelectList<TOption, TOption>(sProps);
	    }

        public IHtmlString SelectList<TOptionKey, TOptionValue>(
            SelectList<TOptionKey, TOptionValue> sProps)
        {
            this.Html.RegisterControl(sProps);

            var classes = RabbitHelper.AddClass(null, "form-control");
            RabbitHelper.AddClass(classes, FormatInputTextSize(sProps.TextSize));

            var attrs = RabbitHelper.AddAttribute(null, "id", sProps.Id);
            RabbitHelper.AddAttribute(attrs, "name", sProps.Name);
            RabbitHelper.AddNotEmptyAttribute(attrs, "disabled", this.IsControlEnabled(sProps.Enabled) ? null : "disabled");
            RabbitHelper.AddNotEmptyAttribute(attrs, "multiple", sProps.AllowMultiple ? "multiple" : null);
            RabbitHelper.AddAttribute(attrs, "class", RabbitHelper.MakeClassAttributeContent(classes));

            var output = string.Format("<select {0}>\n", RabbitHelper.MakeAttributesList(attrs));

            if (sProps.IncludeBlank)
            {
                output += string.Format("<option value=\"\">{0}</option>\n", sProps.BlankText);
            }

            if (sProps.Options != null)
            {
                foreach (var item in sProps.Options)
                {
                    var selected = sProps.SelectedOptions != null && sProps.SelectedOptions.Contains(item.Key);

                    output += string.Format(
                        "<option value=\"{0}\"{1}/>{2}</option>\n",
                        item.Key,
                        selected ? " selected=\"selected\"" : string.Empty,
                        item.Value);
                }
            }

            output += "</select>\n";

            output = this.MakeFormGroup(output, sProps);

            if (!this.goRabbit.Cruncher.HasJsItem("selectList_js"))
            {
                this.goRabbit.Cruncher.AddJsInline(Resources.selectList_js, "selectList_js", OrderOptions.Middle);
            }

            var js = string.Format(
                "new trooper.ui.control.selectList({{id:'{0}', formId:'{1}', warnOnLeave:{2}}});",
                sProps.Id,
                this.FormProps.Id,
                RabbitHelper.GetJsBool(sProps.WarnOnLeave));

            this.goRabbit.Cruncher.AddJsInline(js, OrderOptions.Last);

            return MvcHtmlString.Create(output);
        }

        public IHtmlString SelectListFor<TOption>(
			Expression<Func<TModel, TOption>> expression,
			SelectList<TOption> sProps)
	    {
		    return this.SelectListFor<TOption, TOption>(expression, sProps);
	    }

        public IHtmlString SelectListFor<TOptionKey, TOptionValue>(
            Expression<Func<TModel, TOptionKey>> expression,
            SelectList<TOptionKey, TOptionValue> sProps)
        {
            var value = RabbitHelper.GetExpressionValue<TModel, TOptionKey>(expression, this.goRabbit.HtmlHelper);

			sProps.Name = this.goRabbit.HtmlHelper.NameFor(expression).ToString();
            sProps.Id = this.GetIdFromName(sProps);
            sProps.Messages = this.GetMessagesForProperty(expression, this.FormProps.Messages);
            sProps.SelectedOption = value;

            return this.SelectList(sProps);
        }

        public IHtmlString MultiSelectListFor<TOptionKey, TOptionValue>(
            Expression<Func<TModel, IList<TOptionKey>>> expression,
            SelectList<TOptionKey, TOptionValue> sProps)
        {
            var value = RabbitHelper.GetExpressionValue<TModel, IList<TOptionKey>>(expression, this.goRabbit.HtmlHelper);

			sProps.Name = this.goRabbit.HtmlHelper.NameFor(expression).ToString();
            sProps.Id = this.GetIdFromName(sProps);
            sProps.Messages = this.GetMessagesForProperty(expression, this.FormProps.Messages);
            sProps.SelectedOptions = value;

            return this.SelectList(sProps);
        }

        public IHtmlString RadioList<TOptionKey, TOptionValue>(RadioList<TOptionKey, TOptionValue> rlProps)
        {
            this.Html.RegisterControl(rlProps);

            var output = rlProps.ScrollHeight > 0
                ? string.Format("<div id=\"{0}\" class=\"trooper y-scrolling\" style=\"height: {1}px\">", rlProps.Id, rlProps.ScrollHeight)
                : string.Format("<div id=\"{0}\">", rlProps.Id);

            if (rlProps.Options != null)
            {
                foreach (var item in rlProps.Options)
                {
                    var inpAttrs = RabbitHelper.AddAttribute(null, "type", "radio");
                    RabbitHelper.AddAttribute(inpAttrs, "name", rlProps.Name);
                    RabbitHelper.AddAttribute(inpAttrs, "value", item.Key.ToString());
                    RabbitHelper.AddNotEmptyAttribute(
                        inpAttrs,
                        "checked",
                        item.Value != null && item.Key.Equals(rlProps.SelectedOption) ? "checked" : null);
                    RabbitHelper.AddNotEmptyAttribute(
                        inpAttrs,
                        "disabled",
                        this.IsControlEnabled(rlProps.Enabled) ? null : "disabled");

                    output += string.Format(
                        "<div class=\"radio\"><label class=\"{0}\"><input {1} />{2}</label></div>\n",
                        rlProps.Inline ? "radio-inline" : string.Empty,
                        RabbitHelper.MakeAttributesList(inpAttrs),
                        item.Value);
                }
            }

            output += "</div>";

            output = this.MakeFormGroup(output, rlProps);

            if (!this.goRabbit.Cruncher.HasJsItem("radioList_js"))
            {
                this.goRabbit.Cruncher.AddJsInline(Resources.radioList_js, "radioList_js", OrderOptions.Middle);
            }

            var js = string.Format(
                "new trooper.ui.control.radioList({{id:'{0}', formId:'{1}', name:'{2}', warnOnLeave:{3}}});",
                rlProps.Id,
                this.FormProps.Id,
                rlProps.Name,
                RabbitHelper.GetJsBool(rlProps.WarnOnLeave));

            this.goRabbit.Cruncher.AddJsInline(js, OrderOptions.Last);

            return MvcHtmlString.Create(output);
        }

        public IHtmlString RadioListFor<TOptionKey, TOptionValue>(
            Expression<Func<TModel, TOptionKey>> expression,
            RadioList<TOptionKey, TOptionValue> rlProps)
        {
            var value = RabbitHelper.GetExpressionValue<TModel, TOptionKey>(expression, this.goRabbit.HtmlHelper);

			rlProps.Name = this.goRabbit.HtmlHelper.NameFor(expression).ToString();
            rlProps.Id = this.GetIdFromName(rlProps);
            rlProps.Messages = this.GetMessagesForProperty(expression, this.FormProps.Messages);
            rlProps.SelectedOption = value;

            return this.RadioList(rlProps);
        }

        public IHtmlString DateTimePicker(DateTimePicker dtpProps)
        {
            this.Html.RegisterControl(dtpProps);

	        var icon =
		        new[] {DateTimeFormat.DateAndTime, DateTimeFormat.Date, DateTimeFormat.DateTimeNoSeconds}.Contains(
			        dtpProps.DateTimeFormat)
			        ? "calendar"
			        : "time";

            string level;

            switch (this.GetControlAlertLevel(dtpProps))
            {
                case MessageAlertLevel.Error:
                    level = "danger";
                    break;
                case MessageAlertLevel.Warning:
                    level = "warning";
                    break;
                default:
                    level = "default";
                    break;
            }

	        const string serverFormat = "yyyy-MM-dd HH:mm:ss";

	        var value = dtpProps.Value == null ? string.Empty : ((DateTime) dtpProps.Value).ToString(serverFormat);

	        var result = "<div id=\"" + dtpProps.Id + "\" class=\"trooper dateTimePicker\">\n"
	                     + "<input type=\"hidden\" type=\"text\" name=\"" + dtpProps.Name + "\" value=\"" + value + "\"/>"
	                     + "<div class=\"input-group\">\n" + "<div class=\"form-control datetime-input "
	                     + (dtpProps.TextSize == null ? string.Empty : FormatInputTextSize(dtpProps.TextSize)) + "\""
						 + (this.IsControlEnabled(dtpProps.Enabled) ? " contentEditable =\"true\"" : string.Empty) + "></div>\n";

            if (this.IsControlEnabled(dtpProps.Enabled))
            {
	            result += "<span class=\"input-group-btn\">\n"
	                      + "<button class=\"btn btn-" + level + " date-select\" type=\"button\">\n"
	                      + "<i class=\"glyphicon glyphicon-" + icon + "\">\n</i>"
	                      + "<button class=\"btn btn-" + level + " date-delete\" type=\"button\">\n"
	                      + "<i class=\"glyphicon glyphicon-remove-circle\">"
	                      + "</i>\n</button>\n</span>";
            }

            result += "\n</div>\n</div>\n";

            result = this.MakeFormGroup(result, dtpProps);

            var poProps = new Popover
	        {
		        Selector = string.Format("#{0} .date-select", dtpProps.Id),
		        Behaviour = PopoverBehaviour.ClickThenClickOutside,
				Placement = PopoverPlacements.Top,
				PlacementAutoAssist = true,
	        };

            this.Html.Popover(poProps);

			this.Html.IncludeMoment();
            this.Html.IncludeJqueryInputMask();

            var js = string.Format(
                "new trooper.ui.control.dateTimePicker("
                + "{{id:'{0}', formId:'{1}', dateTimeFormat:'{2}', warnOnLeave:{3}, popoverPlacement:'{4}', utcOffset:{5}, popoverId:'{6}', minimum:{7}, maximum:{8}}});",
                dtpProps.Id,
                this.FormProps.Id,
				dtpProps.DateTimeFormat,
                RabbitHelper.GetJsBool(dtpProps.WarnOnLeave),
                RabbitHelper.PopoverPlacementToString(dtpProps.PopoverPlacement),
                dtpProps.UtcOffset,
                poProps.Id,
				dtpProps.Minimum == null ? "null" : string.Format("'{0:" + serverFormat + "}'", dtpProps.Minimum),
				dtpProps.Maximum == null ? "null" : string.Format("'{0:"+ serverFormat+"}'", dtpProps.Maximum));

			if (!this.goRabbit.Cruncher.HasJsItem("dateTimePicker_js"))
			{
				this.Html.IncludeMoment();
				this.goRabbit.Cruncher.AddJsInline(Resources.dateTimePicker_js, "dateTimePicker_js", OrderOptions.Middle);
				this.goRabbit.Cruncher.AddLessInline(Resources.dateTimePicker_less, "dateTimePicker_less", OrderOptions.Middle);
			}

            this.goRabbit.Cruncher.AddJsInline(js, OrderOptions.Last);

            return MvcHtmlString.Create(result);
        }

        public IHtmlString DateTimePickerFor<TValue>(
            Expression<Func<TModel, TValue>> expression,
            DateTimePicker dtpProps)
        {
            var value = Conversion.ConvertToDateTime(RabbitHelper.GetExpressionValue<TModel, TValue>(expression, this.goRabbit.HtmlHelper));

			dtpProps.Name = this.goRabbit.HtmlHelper.NameFor(expression).ToString();
            dtpProps.Id = this.GetIdFromName(dtpProps);
            dtpProps.Messages = this.GetMessagesForProperty(expression, this.FormProps.Messages);
            dtpProps.Value = value;

            return this.DateTimePicker(dtpProps);
        }

        public IHtmlString Table<T>(TableControl<T> tProps)
            where T : class
        {
            tProps.FormId = this.FormProps.Id;

            return new Table<T>(tProps, this.Html, this.goRabbit.Cruncher).Render();
        }

        public IHtmlString TableFor<T>(Expression<Func<TModel, TableModel>> expression, TableControl<T> tProps)
            where T : class
        {
            var value = RabbitHelper.GetExpressionValue<TModel, TableModel>(expression, this.goRabbit.HtmlHelper);

            tProps.Name = this.goRabbit.HtmlHelper.NameFor(expression).ToString();
            tProps.Id = this.GetIdFromName(tProps);
            tProps.Messages = this.GetMessagesForProperty(expression, this.FormProps.Messages);
            tProps.TableModel = value;

            return this.Table(tProps);
        }

        public IHtmlString SearchBox(SearchBox sbProps)
        {
            this.Html.RegisterControl(sbProps);

            if (!this.goRabbit.Cruncher.HasJsItem("searchBox_js"))
            {
                this.goRabbit.Cruncher.AddJsInline(Resources.searchBox_js, "searchBox_js", OrderOptions.Middle);
                this.goRabbit.Cruncher.AddLessInline(Resources.searchBox_less, "searchBox_less", OrderOptions.Middle);
            }

            var result = string.Format(
                "<input type=\"hidden\" id=\"{0}\" name=\"{1}\" value=\"{2}\" />\n", sbProps.Id, sbProps.Name, sbProps.Value);
            result += string.Format("<div class=\"trooper searchBox\" id=\"{0}\">\n", sbProps.Id);

            result += string.Format(
                "<input type=\"text\" id=\"{0}_search\" name=\"{1}.search\" class=\"form-control {2}\" value=\"{3}\" {4} autocomplete=\"off\" />\n",
                sbProps.Id,
                sbProps.Name,
                sbProps.TextSize == null ? string.Empty : FormatInputTextSize(sbProps.TextSize),
                sbProps.Text,
                this.IsControlEnabled(sbProps.Enabled) ? string.Empty : "readonly=\"readonly\"");

            result += "</div>\n";

            result = this.MakeFormGroup(result, sbProps);

            var js = string.Format(
                "new trooper.ui.control.searchBox("
                + "{{id:'{0}', formId:'{1}', name: '{2}', selectEvent: {3}, dataSourceUrl: '{4}', "
                + "searchValueField: '{5}', searchTextField: '{6}', selectedTextField: '{7}', scrollHeight: {8}, "
                + "popoverPlacement: '{9}', popoutWidth: {10} }});",
                sbProps.Id,
                this.FormProps.Id,
                sbProps.Name,
                string.IsNullOrEmpty(sbProps.SelectEvent) ? "null" : sbProps.SelectEvent,
                sbProps.DataSourceUrl,
                sbProps.SearchValueField,
                sbProps.SearchTextField,
                sbProps.SelectedTextField,
                sbProps.ScrollHeight,
                RabbitHelper.PopoverPlacementToString(sbProps.PopoverPlacement),
                sbProps.PopoutWidth == null ? "null" : sbProps.PopoutWidth.ToString());

            this.goRabbit.Cruncher.AddJsInline(js, OrderOptions.Last);

            return MvcHtmlString.Create(result);
        }

        public IHtmlString SearchBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, SearchBox sbProps)
        {
			sbProps.Name = this.goRabbit.HtmlHelper.NameFor(expression).ToString();
            sbProps.Id = this.GetIdFromName(sbProps);
            sbProps.Messages = this.GetMessagesForProperty(expression, this.FormProps.Messages);

            return this.SearchBox(sbProps);
        }

        #endregion

        #endregion

        #region private static methods

        /// <summary>
        /// Determine the class string for the supplied text size
        /// </summary>
        /// <param name="textSize">The text size</param>
        /// <returns>The CSS string representation</returns>
        private static string FormatInputTextSize(InputTextSizes? textSize = null)
        {
            switch (textSize)
            {
                case InputTextSizes.Large:
                    return "input-lg";
                case InputTextSizes.Default:
                    return string.Empty;
                case InputTextSizes.Small:
                    return "input-sm";
                default:
                    return string.Empty;
            }
        }

        private string MakeFormGroup(string contents, FormControl cBase)
        {
            var classes = new List<string> { "form-group" };

            var worstLevel = this.GetControlAlertLevel(cBase);

            if (cBase.WorstMessageLevel != null)
            {
                RabbitHelper.AddClass(classes, string.Format("has-{0}", worstLevel.ToString().ToLower()));
            }

            RabbitHelper.AddClasses(classes, cBase.FormGroupClasses);

            if (this.IsTitleShowing(cBase.ShowTitle, cBase.Title))
            {
                return
                    string.Format(
                        "<div {0}>\n" + "<label class=\"control-label\" for=\"{1}\">{2}</label>\n" + "{3}" + "</div>\n",
                        RabbitHelper.MakeClassAttribute(classes),
                        cBase.Id,
                        cBase.Title,
                        contents);
            }

            return string.Format("<div {0}>\n{1}</div>\n", RabbitHelper.MakeClassAttribute(classes), contents);
        }

        private string MakeInputGroup(string contents) 
        {
            return string.Format("<div class=\"input-group\">{0}</div>\n", contents);
        }

        private string MakeTextBox(TextBox tbProps, bool incJs)
        {
            //// first lets add our javascript core and control instance
            if (!this.goRabbit.Cruncher.HasJsItem("textBox_js"))
            {
                this.goRabbit.Cruncher.AddJsInline(Resources.textBox_js, "textBox_js", OrderOptions.Middle);
            }

            if (incJs)
            {
                var js = string.Format(
                    "new trooper.ui.control.textBox({{id:'{0}', formId:'{1}', maxLength:{2}, warnOnLeave:{3}}});",
                    tbProps.Id,
                    this.FormProps.Id,
                    Conversion.ConvertToInt(tbProps.MaxLength, 0),
                    RabbitHelper.GetJsBool(tbProps.WarnOnLeave));

                this.goRabbit.Cruncher.AddJsInline(js, OrderOptions.Last);
            }

            //// Then lets get our text box attributes 
            var textBoxAttrs = new Dictionary<string, object>();

            var cssClasses = RabbitHelper.AddClass(null, "form-control");

            if (tbProps.TextSize != null)
            {
                cssClasses = RabbitHelper.AddClass(cssClasses, FormatInputTextSize(tbProps.TextSize));
            }

            if (!this.IsControlEnabled(tbProps.Enabled))
            {
                textBoxAttrs.Add("readonly", "readonly");
            }

            if (tbProps.MaxLength != null && tbProps.MaxLength > 0)
            {
                textBoxAttrs.Add("maxlength", tbProps.MaxLength);
            }

            //// Lets make the control
            var result = string.Format(
                "<input type=\"text\" id=\"{0}\" name=\"{1}\" value=\"{2}\" {3} {4} />\n",
                tbProps.Id,
                tbProps.Name,
                HttpUtility.HtmlAttributeEncode(tbProps.Value),
                RabbitHelper.MakeClassAttribute(cssClasses),
                textBoxAttrs.Aggregate(
                    string.Empty,
                    (current, next) =>
                    current
                    + string.Format(
                        " {0}=\"{1}\"", next.Key, HttpUtility.HtmlAttributeEncode(next.Value.ToString()))));

            return result;
        }

        private string FormatDecimal(decimal? value, int? decimalPlaces)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (decimalPlaces == null)
            {
                return value.ToString();
            }

            var format = string.Format("{{0:F{0}}}", decimalPlaces);

            return string.Format(format, value);
        }

        #endregion

        #region private methods

        private void Init()
        {
            if (!this.goRabbit.Cruncher.HasJsItem("form_js"))
            {
                this.goRabbit.Cruncher.AddJsInline(Resources.form_js, "form_js", OrderOptions.Middle);

                this.goRabbit.Cruncher.AddJsInline(string.Format("new trooper.ui.control.form({{ id: '{0}' }});", this.FormProps.Id), OrderOptions.Last);
            }
        }

        private void IncludeNumericJs()
        {
            if (!this.goRabbit.Cruncher.HasJsItem("numericBox_js"))
            {
                this.goRabbit.Cruncher.AddJsInline(Resources.numericBox_js, "numericBox_js", OrderOptions.Middle);
            }
        }

        /// <summary>
        /// Determine a control should be disabled based on the provided
        /// parameter and class property ControlsEnabled. If disabled
        /// is then class property determines the outcome. 
        /// </summary>
        /// <param name="enabled">The value passed directly to the method</param>
        /// <returns>True if the control should be enabled</returns>
        private bool IsControlEnabled(bool? enabled)
        {
            if (enabled != null)
            {
                return (bool)enabled;
            }

            if (this.FormProps.ControlsEnabled != null)
            {
                return (bool)this.FormProps.ControlsEnabled;
            }

            return true;
        }

        private bool IsTitleShowing(bool? showTitle, string title)
        {
	        if (title == null)
	        {
		        return false;
	        }

            if (showTitle != null)
            {
                return (bool)showTitle;
            }

            if (this.FormProps.ShowTitles != null)
            {
                return (bool)this.FormProps.ShowTitles;
            }

            return true;
        }
        
        private string GetIdFromName(FormControl control)
        {
            return string.IsNullOrEmpty(control.Id) ? control.Name.Replace('.', '_') : control.Id;
        }

        /// <summary>
        /// From the validation results provided and the expression that evaluates to a property it is 
        /// determined if the property is valid.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <param name="messages">
        /// The errors.
        /// </param>
        /// <typeparam name="TValue">
        /// The data type of the expression
        /// </typeparam>
        /// <returns>
        /// The list of messages for the property.
        /// </returns>
        private List<IMessage> GetMessagesForProperty<TValue>(Expression<Func<TModel, TValue>> expression, IEnumerable<IMessage> messages)
        {
            if (messages == null)
            {
                return null;
            }

            var metaData = ModelMetadata.FromLambdaExpression(expression, this.goRabbit.HtmlHelper.ViewData);

            if (metaData == null)
            {
                return null;
            }

            return messages.Where(v => v.Property == metaData.PropertyName).ToList();
        }

        private MessageAlertLevel? GetControlAlertLevel(FormControl cBase)
        {
            var worstControlLevel = cBase.WorstMessageLevel;

            var worstPageLevel = this.FormProps.Messages == null
                                     ? null
                                     : MessageUtility.GetWorstMessageLevel(
                                         this.FormProps.Messages.Where(m => m != null && m.Property == cBase.Name).ToList());

            var levels = new List<MessageAlertLevel?> { worstControlLevel, worstPageLevel };

            return MessageUtility.GetWorstMessageLevel(levels);
        }

        #endregion        
    }
}