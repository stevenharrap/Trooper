//--------------------------------------------------------------------------------------
// <copyright file="Form.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards..
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Bootstrap
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
    public class Form<TModel> : Html<TModel>
    {
        public FormHeader FormHeaderProps { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Form{TModel}"/> class.
		/// By instantiating this the Html class will inject the JQuery and Bootstrap JS and CSS classes.
		/// </summary>
		/// <param name="htmlHelper">
		/// The html helper.
		/// </param>
		/// <param name="fhProps"></param>
		public Form(HtmlHelper<TModel> htmlHelper, FormHeader fhProps)
            : base(htmlHelper)
        {
            this.FormHeaderProps = fhProps;

            this.RegisterControl(this.FormHeaderProps);

            this.Init();
        }

        #region public properties

        /// <summary>
        /// Gets or sets if all controls that support the disabled or readonly parameter 
        /// enabled or disabled (readonly in some cases) by default.
        /// Giving a control a specific disabled or readonly value will over-ride this.
        /// By default this is null and so are the enabled/readonly properties so
        /// result state of a control will be that it is access-able by default.
        /// </summary>
        public bool? ControlsEnabled { get; set; }

        public bool? ShowTitles { get; set; }

        #endregion

        #region public methods

        #region form

        public MvcHtmlString BeginForm()
        {
            var tag = string.Format(
                "<form id=\"{0}\" action=\"{1}\" method=\"{2}\">", 
                this.FormHeaderProps.Id, 
                this.FormHeaderProps.Action, 
                this.FormHeaderProps.Method);

            return new MvcHtmlString(tag);
        }

        public MvcHtmlString EndForm()
        {
            return new MvcHtmlString("</form>");
        }

        #endregion

        #region controls

        public MvcHtmlString TextBox(TextBox tbProps)
        {
            this.RegisterControl(tbProps);

            var result = this.MakeTextBox(tbProps, true);

            result = this.MakeFormGroup(result, tbProps);

            return MvcHtmlString.Create(result);
        }

        public MvcHtmlString TextBoxFor<TValue>(
            Expression<Func<TModel, TValue>> expression, 
            TextBox tbProps)
        {
            var value = Conversion.ConvertToString(this.GetExpressionValue(expression));

	        tbProps.Name = this.HtmlHelper.NameFor(expression).ToString();
            tbProps.Id = this.GetIdFromName(tbProps);
            tbProps.Messages = this.GetMessagesForProperty(expression, this.Messages);
            tbProps.Value = value;

            return this.TextBox(tbProps);
        }
        
        public MvcHtmlString IntegerBox(IntegerBox iProps)
        {
            this.RegisterControl(iProps);

            var tbProps = iProps as TextBox;

            tbProps.Value = iProps.Value == null ? string.Empty : iProps.Value.ToString();

            var result = this.MakeTextBox(tbProps, false);

            result = this.MakeFormGroup(result, iProps);

            this.IncludeNumericJs();

            var js = string.Format(
                "new trooper.ui.control.numericBox({{id:'{0}', formId:'{1}', numericType:'Integer', minimum:{2}, maximum:{3}, decimalDigits:0}});",
                iProps.Id,
                this.FormHeaderProps.Id,
                iProps.Minimum == null ? "null" : Conversion.ConvertToString(iProps.Minimum),
                iProps.Maximum == null ? "null" : Conversion.ConvertToString(iProps.Maximum));

            this.Cruncher.AddJsInline(js, OrderOptions.Last);

            return MvcHtmlString.Create(result);
        }

        public MvcHtmlString IntegerBoxFor<TValue>(
            Expression<Func<TModel, TValue>> expression,
            IntegerBox iProps)
        {
            var value = Conversion.ConvertToInt(this.GetExpressionValue(expression));

			iProps.Name = this.HtmlHelper.NameFor(expression).ToString();
            iProps.Id = this.GetIdFromName(iProps);
            iProps.Messages = this.GetMessagesForProperty(expression, this.Messages);
            iProps.Value = value;

            return this.IntegerBox(iProps);
        }

        public MvcHtmlString DecimalBox(DecimalBox dProps)
        {
            this.RegisterControl(dProps);

            var tbProps = dProps as TextBox;

            tbProps.Value = this.FormatDecimal(dProps.Value, dProps.DecimalDigits);

            var output = this.MakeTextBox(tbProps, false);

            output = this.MakeFormGroup(output, dProps);
            
            this.IncludeNumericJs();

            var js = string.Format(
                "new trooper.ui.control.numericBox({{id:'{0}', formId:'{1}', numericType:'Decimal', minimum:{2}, maximum:{3}, decimalDigits:{4}}});",
                dProps.Id,
                this.FormHeaderProps.Id,
                dProps.Minimum == null ? "null" : Conversion.ConvertToString(dProps.Minimum),
                dProps.Maximum == null ? "null" : Conversion.ConvertToString(dProps.Maximum),
				dProps.DecimalDigits == null ? "null" : Conversion.ConvertToString(dProps.DecimalDigits));

            this.Cruncher.AddJsInline(js, OrderOptions.Last);

            return MvcHtmlString.Create(output);
        }

        public MvcHtmlString DecimalBoxFor<TValue>(
            Expression<Func<TModel, TValue>> expression,
            DecimalBox dProps)
        {
            var value = Conversion.ConvertToDecimal(this.GetExpressionValue(expression));

			dProps.Name = this.HtmlHelper.NameFor(expression).ToString();
            dProps.Id = this.GetIdFromName(dProps);
            dProps.Messages = this.GetMessagesForProperty(expression, this.Messages);
            dProps.Value = value;

            return this.DecimalBox(dProps);
        }

        public MvcHtmlString PercentageBox(DecimalBox pProps)
        {
            this.RegisterControl(pProps);
            
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
                this.FormHeaderProps.Id,
                pProps.Minimum == null ? "null" : Conversion.ConvertToString(pProps.Minimum),
                pProps.Maximum == null ? "null" : Conversion.ConvertToString(pProps.Maximum),
                pProps.DecimalDigits);

            this.Cruncher.AddJsInline(js, OrderOptions.Last);

            return MvcHtmlString.Create(output);
        }

        public MvcHtmlString PercentageBoxFor<TValue>(
            Expression<Func<TModel, TValue>> expression,
            DecimalBox dProps)
        {
            var value = Conversion.ConvertToDecimal(this.GetExpressionValue(expression));

			dProps.Name = this.HtmlHelper.NameFor(expression).ToString();
            dProps.Id = this.GetIdFromName(dProps);
            dProps.Messages = this.GetMessagesForProperty(expression, this.Messages);
            dProps.Value = value;

            return this.PercentageBox(dProps);
        }

        public MvcHtmlString CurrencyBox(DecimalBox cProps)
        {
            this.RegisterControl(cProps);

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
                this.FormHeaderProps.Id,
                cProps.Minimum == null ? "null" : Conversion.ConvertToString(cProps.Minimum),
                cProps.Maximum == null ? "null" : Conversion.ConvertToString(cProps.Maximum),
                cProps.DecimalDigits ?? 0);

            this.Cruncher.AddJsInline(js, OrderOptions.Last);

            return MvcHtmlString.Create(output);
        }

        public MvcHtmlString CurrencyBoxFor<TValue>(
            Expression<Func<TModel, TValue>> expression,
            DecimalBox dProps)
        {
            var value = Conversion.ConvertToDecimal(this.GetExpressionValue(expression));

			dProps.Name = this.HtmlHelper.NameFor(expression).ToString();
            dProps.Id = this.GetIdFromName(dProps);
            dProps.Messages = this.GetMessagesForProperty(expression, this.Messages);
            dProps.Value = value;

            return this.CurrencyBox(dProps);
        }

        public MvcHtmlString TextareaBox(TextareaBox tabProps)
        {
            this.RegisterControl(tabProps);

            if (!this.Cruncher.HasJsItem("textareaBox_js"))
            {
                this.Cruncher.AddJsInline(Resources.textareaBox_js, "textareaBox_js", OrderOptions.Middle);
            }

            var js = string.Format(
                "new trooper.ui.control.textareaBox({{id:'{0}', formId:'{1}', maxLength:{2}, warnOnLeave:{3}}});",
                tabProps.Id,
                this.FormHeaderProps.Id,
                Conversion.ConvertToInt(tabProps.MaxLength, 0),
                this.GetJsBool(tabProps.WarnOnLeave));

            this.Cruncher.AddJsInline(js, OrderOptions.Last);            

            var textBoxAttrs = new Dictionary<string, object>();

            var cssClasses = this.AddClass(null, "form-control");

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
                this.MakeClassAttribute(cssClasses),
                textBoxAttrs.Aggregate(
                    string.Empty,
                    (current, next) =>
                    current
                    + string.Format(
                        " {0}=\"{1}\"", next.Key, HttpUtility.HtmlAttributeEncode(next.Value.ToString()))));

            result = this.MakeFormGroup(result, tabProps);

            return MvcHtmlString.Create(result);
        }

        public MvcHtmlString TextareaBoxFor<TValue>(
            Expression<Func<TModel, TValue>> expression,
            TextareaBox tabProp)
        {
            var value = Conversion.ConvertToString(this.GetExpressionValue(expression));

			tabProp.Name = this.HtmlHelper.NameFor(expression).ToString();
            tabProp.Id = this.GetIdFromName(tabProp);
            tabProp.Messages = this.GetMessagesForProperty(expression, this.Messages);
            tabProp.Value = value;

            return this.TextareaBox(tabProp);
        }

        public MvcHtmlString Button(Button bProps)
        {
            this.RegisterControl(bProps);

            if (!this.Cruncher.HasJsItem("button_js"))
            {
                this.Cruncher.AddJsInline(Resources.button_js, "button_js", OrderOptions.Middle);
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
                this.FormHeaderProps.Id,
                bProps.Url ?? string.Empty,
                this.GetJsBool(bProps.TargetNewWindow),
                this.GetJsBool(bProps.LaunchLoadingOnclick),
                bProps.LoadingScreenTitle ?? string.Empty,
                this.GetJsBool(bProps.ConfirmOnClick),
                bProps.ConfirmTitle ?? string.Empty,
                this.GetJsBool(bProps.Submit));

            this.Cruncher.AddJsInline(js, OrderOptions.Last);

            var buttonClasses = this.AddClasses(null, bProps.ButtonClasses);
            this.AddClass(buttonClasses, this.ButtonTypeToString(bProps.ButtonType));
            this.AddClass(buttonClasses, "btn");
            this.AddClass(buttonClasses, bProps.Visible ? string.Empty : "hidden");

            var attrs = this.AddAttributes(null, bProps.Attrs);
            this.AddAttribute(attrs, "type", bProps.Submit ? "submit" : "button");
            this.AddAttribute(attrs, "id", bProps.Id);
            this.AddAttribute(attrs, "name", bProps.Name);
            this.AddAttribute(attrs, "value", bProps.Value);
            this.AddNotEmptyAttribute(attrs, "disabled", this.IsControlEnabled(bProps.Enabled) ? string.Empty : "disabled");
            this.AddNotEmptyAttribute(attrs, "title", bProps.ToolTip);
            this.AddAttribute(attrs, "class", this.MakeClassAttributeContent(buttonClasses));

            var output = string.Format(
                "<button {0}>{1}{2}</a>",
                this.MakeAttributesList(attrs),
                string.IsNullOrEmpty(bProps.Icon)
                    ? string.Empty
                    : string.Format("{0} ", this.MakeIcon(bProps.Icon)),
                bProps.Title);

            return MvcHtmlString.Create(output);
        }
        
        public MvcHtmlString ButtonFor<TValue>(Expression<Func<TModel, TValue>> expression, Button bProps)
        {
            var value = Conversion.ConvertToString(this.GetExpressionValue(expression));

			bProps.Name = this.HtmlHelper.NameFor(expression).ToString();
            bProps.Id = this.GetIdFromName(bProps);
            bProps.Messages = this.GetMessagesForProperty(expression, this.Messages);
            bProps.Value = value;

            return this.Button(bProps);
        }

        public MvcHtmlString Upload(UploadBox ubProps)
        {
            this.RegisterControl(ubProps);

            if (ubProps.UploadModel == null)
            {
                return MvcHtmlString.Create(string.Empty);
            }

            var action = this.UrlHelper.Action("OpenIframe", "BsUpload", new { id = ubProps.Id });

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

            if (!this.Cruncher.HasJsItem("upload_js"))
            {
                this.Cruncher.AddJsInline(Resources.upload_js, "upload_js", OrderOptions.Middle);

                this.Cruncher.AddLessInline(Resources.upload_less, "upload_less", OrderOptions.Middle);
            }

            var js = string.Format(
                "new trooper.ui.control.upload({{id:'{0}', formId:'{1}', warnOnLeave: {2}}});",
                ubProps.Id,
                this.FormHeaderProps.Id,
                this.GetJsBool(ubProps.WarnOnLeave));

            this.Cruncher.AddJsInline(js, OrderOptions.Last);

            return MvcHtmlString.Create(output);
        }

        public MvcHtmlString UploadFor(Expression<Func<TModel, UploadModel>> expression, UploadBox ubProps)
        {
            var value = this.GetExpressionValue(expression);

			ubProps.Name = this.HtmlHelper.NameFor(expression).ToString();
            ubProps.Id = this.GetIdFromName(ubProps);
            ubProps.Messages = this.GetMessagesForProperty(expression, this.Messages);
            ubProps.UploadModel = value;

            return this.Upload(ubProps);
        }

        public MvcHtmlString CheckBoxList<TOptionKey, TOptionValue>(CheckBoxList<TOptionKey, TOptionValue> cblProps)
        {
            this.RegisterControl(cblProps);

            var output = cblProps.ScrollHeight > 0
                ? string.Format("<div id=\"{0}\" class=\"trooper y-scrolling\" style=\"height: {1}px\">", cblProps.Id, cblProps.ScrollHeight)
                : string.Format("<div id=\"{0}\">", cblProps.Id);
            
            if (cblProps.Options != null)
            {
                foreach (var item in cblProps.Options)
                {
                    var inpAttrs = this.AddAttribute(null, "type", "checkbox");
                    this.AddAttribute(inpAttrs, "name", cblProps.Name);
                    this.AddAttribute(inpAttrs, "value", item.Key.ToString());
                    this.AddNotEmptyAttribute(
                        inpAttrs,
                        "checked",
                        item.Value != null && cblProps.SelectedOptions != null && cblProps.SelectedOptions.Contains(item.Key) ? "checked" : null);
                    this.AddNotEmptyAttribute(
                        inpAttrs,
                        "disabled",
                        this.IsControlEnabled(cblProps.Enabled) ? null : "disabled");
                    
                    output += string.Format(
                        "<div class=\"checkbox\"><label class=\"{0}\"><input {1} />{2}</label></div>\n",
                        cblProps.Inline ? "checkbox-inline" : string.Empty, 
                        this.MakeAttributesList(inpAttrs), 
                        item.Value);
                }
            }
            
            output += "</div>";

            output = this.MakeFormGroup(output, cblProps);

            if (!this.Cruncher.HasJsItem("checkBoxList_js"))
            {
                this.Cruncher.AddJsInline(Resources.checkBoxList_js, "checkBoxList_js", OrderOptions.Middle);
            }

            var js = string.Format(
                "new trooper.ui.control.checkBoxList({{id:'{0}', formId:'{1}', name:'{2}', warnOnLeave:{3} }});",
                cblProps.Id,
                this.FormHeaderProps.Id,
                cblProps.Name,
                this.GetJsBool(cblProps.WarnOnLeave));

            this.Cruncher.AddJsInline(js, OrderOptions.Middle);

            return MvcHtmlString.Create(output);
        }

        public MvcHtmlString CheckBoxListFor<TOptionKey, TOptionValue>(
            Expression<Func<TModel, List<TOptionKey>>> expression,
            CheckBoxList<TOptionKey, TOptionValue> cblProps)
        {
            var value = this.GetExpressionValue(expression);

			cblProps.Name = this.HtmlHelper.NameFor(expression).ToString();
            cblProps.Id = this.GetIdFromName(cblProps);
            cblProps.Messages = this.GetMessagesForProperty(expression, this.Messages);
            cblProps.SelectedOptions = value;

            return this.CheckBoxList(cblProps);
        }

        public MvcHtmlString CheckBox(CheckBox cbProps)
        {
            this.RegisterControl(cbProps);

            var inpAttrs = this.AddAttribute(null, "type", "checkbox");
            this.AddAttribute(inpAttrs, "name", cbProps.Name);
            this.AddAttribute(inpAttrs, "value", cbProps.Value);
            this.AddNotEmptyAttribute(inpAttrs, "checked", (cbProps.Checked ?? false) ? "checked" : null);
            this.AddNotEmptyAttribute(inpAttrs, "disabled", this.IsControlEnabled(cbProps.Enabled) ? null : "disabled");

            var output = string.Format(
                "<div class=\"checkbox\"><label class=\"{0}\"><input {1} />{2}</label></div>\n",
                cbProps.Inline ? "checkbox-inline" : string.Empty,
                this.MakeAttributesList(inpAttrs),
                cbProps.Title);

            if (!this.Cruncher.HasJsItem("checkBox_js"))
            {
                this.Cruncher.AddJsInline(Resources.checkBox_js, "checkBox_js", OrderOptions.Middle);
            }

            var js = string.Format(
                "new trooper.ui.control.checkBox({{id:'{0}', formId:'{1}', warnOnLeave:{2}}});",
                cbProps.Id,
                this.FormHeaderProps.Id,
                this.GetJsBool(cbProps.WarnOnLeave));

            this.Cruncher.AddJsInline(js, OrderOptions.Last);

            return MvcHtmlString.Create(output);
        }

        public MvcHtmlString CheckBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, CheckBox cbProps)
        {
            var value = Conversion.ConvertToBoolean(this.GetExpressionValue(expression));

			cbProps.Name = this.HtmlHelper.NameFor(expression).ToString();
            cbProps.Id = this.GetIdFromName(cbProps);
            cbProps.Messages = this.GetMessagesForProperty(expression, this.Messages);
            cbProps.Checked = value;

            return this.CheckBox(cbProps);
        }

	    public MvcHtmlString SelectList<TOption>(
			SelectList<TOption> sProps)
	    {
		    return this.SelectList<TOption, TOption>(sProps);
	    }

	    public MvcHtmlString SelectList<TOptionKey, TOptionValue>(
            SelectList<TOptionKey, TOptionValue> sProps)
        {
            this.RegisterControl(sProps);

            var classes = this.AddClass(null, "form-control");
            this.AddClass(classes, FormatInputTextSize(sProps.TextSize));

            var attrs = this.AddAttribute(null, "id", sProps.Id);
            this.AddAttribute(attrs, "name", sProps.Name);
            this.AddNotEmptyAttribute(attrs, "disabled", this.IsControlEnabled(sProps.Enabled) ? null : "disabled");
            this.AddNotEmptyAttribute(attrs, "multiple", sProps.AllowMultiple ? "multiple" : null);
            this.AddAttribute(attrs, "class", this.MakeClassAttributeContent(classes));

            var output = string.Format("<select {0}>\n", this.MakeAttributesList(attrs));

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

            if (!this.Cruncher.HasJsItem("selectList_js"))
            {
                this.Cruncher.AddJsInline(Resources.selectList_js, "selectList_js", OrderOptions.Middle);
            }

            var js = string.Format(
                "new trooper.ui.control.selectList({{id:'{0}', formId:'{1}', warnOnLeave:{2}}});",
                sProps.Id,
                this.FormHeaderProps.Id,
                this.GetJsBool(sProps.WarnOnLeave));

            this.Cruncher.AddJsInline(js, OrderOptions.Last);

            return MvcHtmlString.Create(output);
        }

	    public MvcHtmlString SelectListFor<TOption>(
			Expression<Func<TModel, TOption>> expression,
			SelectList<TOption> sProps)
	    {
		    return this.SelectListFor<TOption, TOption>(expression, sProps);
	    }

        public MvcHtmlString SelectListFor<TOptionKey, TOptionValue>(
            Expression<Func<TModel, TOptionKey>> expression,
            SelectList<TOptionKey, TOptionValue> sProps)
        {
            var value = this.GetExpressionValue(expression);

			sProps.Name = this.HtmlHelper.NameFor(expression).ToString();
            sProps.Id = this.GetIdFromName(sProps);
            sProps.Messages = this.GetMessagesForProperty(expression, this.Messages);
            sProps.SelectedOption = value;

            return this.SelectList(sProps);
        }

        public MvcHtmlString MultiSelectListFor<TOptionKey, TOptionValue>(
            Expression<Func<TModel, List<TOptionKey>>> expression,
            SelectList<TOptionKey, TOptionValue> sProps)
        {
            var value = this.GetExpressionValue(expression);

			sProps.Name = this.HtmlHelper.NameFor(expression).ToString();
            sProps.Id = this.GetIdFromName(sProps);
            sProps.Messages = this.GetMessagesForProperty(expression, this.Messages);
            sProps.SelectedOptions = value;

            return this.SelectList(sProps);
        }

        public MvcHtmlString RadioList<TOptionKey, TOptionValue>(RadioList<TOptionKey, TOptionValue> rlProps)
        {
            this.RegisterControl(rlProps);

            var output = rlProps.ScrollHeight > 0
                ? string.Format("<div id=\"{0}\" class=\"trooper y-scrolling\" style=\"height: {1}px\">", rlProps.Id, rlProps.ScrollHeight)
                : string.Format("<div id=\"{0}\">", rlProps.Id);

            if (rlProps.Options != null)
            {
                foreach (var item in rlProps.Options)
                {
                    var inpAttrs = this.AddAttribute(null, "type", "radio");
                    this.AddAttribute(inpAttrs, "name", rlProps.Name);
                    this.AddAttribute(inpAttrs, "value", item.Key.ToString());
                    this.AddNotEmptyAttribute(
                        inpAttrs,
                        "checked",
                        item.Value != null && item.Key.Equals(rlProps.SelectedOption) ? "checked" : null);
                    this.AddNotEmptyAttribute(
                        inpAttrs,
                        "disabled",
                        this.IsControlEnabled(rlProps.Enabled) ? null : "disabled");

                    output += string.Format(
                        "<div class=\"radio\"><label class=\"{0}\"><input {1} />{2}</label></div>\n",
                        rlProps.Inline ? "radio-inline" : string.Empty,
                        this.MakeAttributesList(inpAttrs),
                        item.Value);
                }
            }

            output += "</div>";

            output = this.MakeFormGroup(output, rlProps);

            if (!this.Cruncher.HasJsItem("radioList_js"))
            {
                this.Cruncher.AddJsInline(Resources.radioList_js, "radioList_js", OrderOptions.Middle);
            }

            var js = string.Format(
                "new trooper.ui.control.radioList({{id:'{0}', formId:'{1}', name:'{2}', warnOnLeave:{3}}});",
                rlProps.Id,
                this.FormHeaderProps.Id,
                rlProps.Name,
                this.GetJsBool(rlProps.WarnOnLeave));

            this.Cruncher.AddJsInline(js, OrderOptions.Last);

            return MvcHtmlString.Create(output);
        }

        public MvcHtmlString RadioListFor<TOptionKey, TOptionValue>(
            Expression<Func<TModel, TOptionKey>> expression,
            RadioList<TOptionKey, TOptionValue> rlProps)
        {
            var value = this.GetExpressionValue(expression);

			rlProps.Name = this.HtmlHelper.NameFor(expression).ToString();
            rlProps.Id = this.GetIdFromName(rlProps);
            rlProps.Messages = this.GetMessagesForProperty(expression, this.Messages);
            rlProps.SelectedOption = value;

            return this.RadioList(rlProps);
        }

        public MvcHtmlString DateTimePicker(DateTimePicker dtpProps)
        {
            this.RegisterControl(dtpProps);

            var format = string.Empty;

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
                        
            var result = "<div id=\"" + dtpProps.Id + "\" class=\"trooper dateTimePicker\">\n"
                         + "<div class=\"input-group\">\n" + "<input class=\"form-control datetime-input "
                         + (dtpProps.TextSize == null ? string.Empty : FormatInputTextSize(dtpProps.TextSize)) + "\""
                         + "\" name=\"" + dtpProps.Name + "\" value=\"" + (dtpProps.Value == null ? string.Empty : ((DateTime)dtpProps.Value).ToString(format))
                         + "\" type=\"text\" " + (this.IsControlEnabled(dtpProps.Enabled) ? string.Empty : "readonly=\"readonly\"") + "/>\n";

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

            this.Popover(poProps);

			this.IncludeMoment();
            this.IncludeJqueryInputMask();

            var js = string.Format(
                "new trooper.ui.control.dateTimePicker("
				+ "{{id:'{0}', formId:'{1}', dateTimeFormat:'{2}', warnOnLeave:{3}, popoverPlacement:'{4}', format:'{5}', timezone:'{6}', popoverId:'{7}'}});",
                dtpProps.Id,
                this.FormHeaderProps.Id,
				dtpProps.DateTimeFormat,
                this.GetJsBool(dtpProps.WarnOnLeave),
                this.PopoverPlacementToString(dtpProps.PopoverPlacement),
                format,
                dtpProps.Timezone,
                poProps.Id);

			if (!this.Cruncher.HasJsItem("dateTimePicker_js"))
			{
				this.IncludeMoment();
				this.Cruncher.AddJsInline(Resources.dateTimePicker_js, "dateTimePicker_js", OrderOptions.Middle);
				this.Cruncher.AddLessInline(Resources.dateTimePicker_less, "dateTimePicker_less", OrderOptions.Middle);
			}

            this.Cruncher.AddJsInline(js, OrderOptions.Last);

            return MvcHtmlString.Create(result);
        }

        public MvcHtmlString DateTimePickerFor<TValue>(
            Expression<Func<TModel, TValue>> expression,
            DateTimePicker dtpProps)
        {
            var value = Conversion.ConvertToDateTime(this.GetExpressionValue(expression));

			dtpProps.Name = this.HtmlHelper.NameFor(expression).ToString();
            dtpProps.Id = this.GetIdFromName(dtpProps);
            dtpProps.Messages = this.GetMessagesForProperty(expression, this.Messages);
            dtpProps.Value = value;

            return this.DateTimePicker(dtpProps);
        }
		
        public MvcHtmlString SearchBox(SearchBox sbProps)
        {
            this.RegisterControl(sbProps);

            if (!this.Cruncher.HasJsItem("searchBox_js"))
            {
                this.Cruncher.AddJsInline(Resources.searchBox_js, "searchBox_js", OrderOptions.Middle);
                this.Cruncher.AddLessInline(Resources.searchBox_less, "searchBox_less", OrderOptions.Middle);
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
                this.FormHeaderProps.Id,
                sbProps.Name,
                string.IsNullOrEmpty(sbProps.SelectEvent) ? "null" : sbProps.SelectEvent,
                sbProps.DataSourceUrl,
                sbProps.SearchValueField,
                sbProps.SearchTextField,
                sbProps.SelectedTextField,
                sbProps.ScrollHeight,
                this.PopoverPlacementToString(sbProps.PopoverPlacement),
                sbProps.PopoutWidth == null ? "null" : sbProps.PopoutWidth.ToString());

            this.Cruncher.AddJsInline(js, OrderOptions.Last);

            return MvcHtmlString.Create(result);
        }

        public MvcHtmlString SearchBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, SearchBox sbProps)
        {
			sbProps.Name = this.HtmlHelper.NameFor(expression).ToString();
            sbProps.Id = this.GetIdFromName(sbProps);
            sbProps.Messages = this.GetMessagesForProperty(expression, this.Messages);

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
                classes = this.AddClass(classes, string.Format("has-{0}", worstLevel.ToString().ToLower()));
            }

            classes = this.AddClasses(classes, cBase.FormGroupClasses);

            if (this.IsTitleShowing(cBase.ShowTitle, cBase.Title))
            {
                return
                    string.Format(
                        "<div {0}>\n" + "<label class=\"control-label\" for=\"{1}\">{2}</label>\n" + "{3}" + "</div>\n",
                        this.MakeClassAttribute(classes),
                        cBase.Id,
                        cBase.Title,
                        contents);
            }

            return string.Format("<div {0}>\n{1}</div>\n", this.MakeClassAttribute(classes), contents);
        }

        private string MakeInputGroup(string contents) 
        {
            return string.Format("<div class=\"input-group\">{0}</div>\n", contents);
        }

        private string MakeTextBox(TextBox tbProps, bool incJs)
        {
            //// first lets add our javascript core and control instance
            if (!this.Cruncher.HasJsItem("textBox_js"))
            {
                this.Cruncher.AddJsInline(Resources.textBox_js, "textBox_js", OrderOptions.Middle);
            }

            if (incJs)
            {
                var js = string.Format(
                    "new trooper.ui.control.textBox({{id:'{0}', formId:'{1}', maxLength:{2}, warnOnLeave:{3}}});",
                    tbProps.Id,
                    this.FormHeaderProps.Id,
                    Conversion.ConvertToInt(tbProps.MaxLength, 0),
                    this.GetJsBool(tbProps.WarnOnLeave));

                this.Cruncher.AddJsInline(js, OrderOptions.Last);
            }

            //// Then lets get our text box attributes 
            var textBoxAttrs = new Dictionary<string, object>();

            var cssClasses = this.AddClass(null, "form-control");

            if (tbProps.TextSize != null)
            {
                cssClasses = this.AddClass(cssClasses, FormatInputTextSize(tbProps.TextSize));
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
                this.MakeClassAttribute(cssClasses),
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
            if (!this.Cruncher.HasJsItem("form_js"))
            {
                this.Cruncher.AddJsInline(Resources.form_js, "form_js", OrderOptions.Middle);

                this.Cruncher.AddJsInline(string.Format("new trooper.ui.control.form({{ id: '{0}' }});", this.FormHeaderProps.Id), OrderOptions.Last);
            }
        }

        private void IncludeNumericJs()
        {
            if (!this.Cruncher.HasJsItem("numericBox_js"))
            {
                this.Cruncher.AddJsInline(Resources.numericBox_js, "numericBox_js", OrderOptions.Middle);
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

            if (this.ControlsEnabled != null)
            {
                return (bool)this.ControlsEnabled;
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

            if (this.ShowTitles != null)
            {
                return (bool)this.ShowTitles;
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

            var metaData = ModelMetadata.FromLambdaExpression(expression, this.HtmlHelper.ViewData);

            if (metaData == null)
            {
                return null;
            }

            return messages.Where(v => v.Property == metaData.PropertyName).ToList();
        }

        private MessageAlertLevel? GetControlAlertLevel(FormControl cBase)
        {
            var worstControlLevel = cBase.WorstMessageLevel;

            var worstPageLevel = this.Messages == null
                                     ? null
                                     : MessageUtility.GetWorstMessageLevel(
                                         this.Messages.Where(m => m != null && m.Property == cBase.Name).ToList());

            var levels = new List<MessageAlertLevel?> { worstControlLevel, worstPageLevel };

            return MessageUtility.GetWorstMessageLevel(levels);
        }

        #endregion
    }
}