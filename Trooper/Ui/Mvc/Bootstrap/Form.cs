﻿//--------------------------------------------------------------------------------------
// <copyright file="Form.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Bootstrap
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;

    using Trooper.BusinessOperation.Business;
    using Trooper.BusinessOperation.Utility;
    using Trooper.Properties;
    using Trooper.Ui.Mvc.Bootstrap.Controls;
    using Trooper.Ui.Mvc.Bootstrap.Models;
    using Trooper.Ui.Mvc.Cruncher;
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
        public Form(HtmlHelper<TModel> htmlHelper, string id)
            : base(htmlHelper)
        {
            this.Id = id;

            this.Init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Form{TModel}"/> class.
        /// By instantiating this the Html class will inject the JQuery and Bootstrap JS and CSS classes.
        /// </summary>
        /// <param name="htmlHelper">
        /// The html helper.
        /// </param>
        public Form(HtmlHelper<TModel> htmlHelper)
            : base(htmlHelper)
        {
            this.Id = "bootstrapform";

            this.Init();
        }

        private void Init()
        {
            if (!this.Cruncher.HeaderJs().HasItem("BootstrapForm_js"))
            {
                this.Cruncher.HeaderJs()
                    .AddInline(Resources.BootstrapForm_js, name: "BootstrapForm_js", order: StoreItem.OrderOptions.Middle);

                this.Cruncher.HeaderJs().AddInline(string.Format("var bootstrapForm = new BootstrapForm({{ id: '{0}' }});", this.Id));
            }
        }

        public string Id { get; set; }

        /// <summary>
        /// Gets or sets if all controls that support the disabled or readonly parameter 
        /// enabled or disabled (readonly in some cases) by default.
        /// Giving a control a specific disabled or readonly value will over-ride this.
        /// By default this is null and so are the enabled/readonly properties so
        /// result state of a control will be that it is access-able by default.
        /// </summary>
        public bool? ControlsEnabled { get; set; }

        public bool? ShowTitles { get; set; }

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

            tbProps.Name = this.GetExpressionAsName(expression);
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

            this.Cruncher.HeaderJs()
                    .AddInline(
                        string.Format(
                            "new BootstrapNumericBox({{id:'{0}', formId:'{1}', numericType:'int', minimum:{2}, maximum:{3}, decimalDigits:0}});",
                            iProps.Id,
                            this.Id,
                            iProps.Minimum == null ? "null" : Conversion.ConvertToString(iProps.Minimum),
                            iProps.Maximum == null ? "null" : Conversion.ConvertToString(iProps.Maximum)));

            return MvcHtmlString.Create(result);
        }

        public MvcHtmlString IntegerBoxFor<TValue>(
            Expression<Func<TModel, TValue>> expression,
            IntegerBox iProps)
        {
            var value = Conversion.ConvertToInt(this.GetExpressionValue(expression));

            iProps.Name = this.GetExpressionAsName(expression);
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

            this.Cruncher.HeaderJs()
                    .AddInline(
                        string.Format(
                            "new BootstrapNumericBox({{id:'{0}', formId:'{1}', numericType:'int', minimum:{2}, maximum:{3}, decimalDigits:{4}}});",
                            dProps.Id,
                            this.Id,
                            dProps.Minimum == null ? "null" : Conversion.ConvertToString(dProps.Minimum),
                            dProps.Maximum == null ? "null" : Conversion.ConvertToString(dProps.Maximum),
                            dProps.DecimalDigits ?? 0));

            return MvcHtmlString.Create(output);
        }

        public MvcHtmlString DecimalBoxFor<TValue>(
            Expression<Func<TModel, TValue>> expression,
            DecimalBox dProps)
        {
            var value = Conversion.ConvertToDecimal(this.GetExpressionValue(expression));

            dProps.Name = this.GetExpressionAsName(expression);
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

            output = this.MakeInputGroup(output + "<span class=\"input-group-addon\">%</span>\n", pProps);

            output = this.MakeFormGroup(output, pProps);

            this.IncludeNumericJs();            

            this.Cruncher.HeaderJs()
                    .AddInline(
                        string.Format(
                            "new BootstrapNumericBox({{id:'{0}', formId:'{1}', numericType:'%', minimum:{2}, maximum:{3}, decimalDigits:{4}}});",
                            pProps.Id,
                            this.Id,
                            pProps.Minimum == null ? "null" : Conversion.ConvertToString(pProps.Minimum),
                            pProps.Maximum == null ? "null" : Conversion.ConvertToString(pProps.Maximum),
                            pProps.DecimalDigits));

            return MvcHtmlString.Create(output);
        }

        public MvcHtmlString PercentageBoxFor<TValue>(
            Expression<Func<TModel, TValue>> expression,
            DecimalBox dProps)
        {
            var value = Conversion.ConvertToDecimal(this.GetExpressionValue(expression));

            dProps.Name = this.GetExpressionAsName(expression);
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

            output = this.MakeInputGroup("<span class=\"input-group-addon\">$</span>\n" + output, cProps);

            output = this.MakeFormGroup(output, cProps);

            this.IncludeNumericJs();

            this.Cruncher.HeaderJs()
                    .AddInline(
                        string.Format(
                            "new BootstrapNumericBox({{id:'{0}', formId:'{1}', numericType:'$', minimum:{2}, maximum:{3}, decimalDigits:{4}}});",
                            cProps.Id,
                            this.Id,
                            cProps.Minimum == null ? "null" : Conversion.ConvertToString(cProps.Minimum),
                            cProps.Maximum == null ? "null" : Conversion.ConvertToString(cProps.Maximum),
                            cProps.DecimalDigits ?? 0));

            return MvcHtmlString.Create(output);
        }

        public MvcHtmlString CurrencyBoxFor<TValue>(
            Expression<Func<TModel, TValue>> expression,
            DecimalBox dProps)
        {
            var value = Conversion.ConvertToDecimal(this.GetExpressionValue(expression));

            dProps.Name = this.GetExpressionAsName(expression);
            dProps.Id = this.GetIdFromName(dProps);
            dProps.Messages = this.GetMessagesForProperty(expression, this.Messages);
            dProps.Value = value;

            return this.CurrencyBox(dProps);
        }

        public MvcHtmlString TextareaBox(TextareaBox tabProps)
        {
            this.RegisterControl(tabProps);

            if (!this.Cruncher.HeaderJs().HasItem("BootstrapTextareaBox_js"))
            {
                this.Cruncher.HeaderJs()
                          .AddInline(
                              Resources.BootstrapTextareaBox_js,
                              name: "BootstrapTextareaBox_js",
                              order: StoreItem.OrderOptions.Middle);
            }

            this.Cruncher.HeaderJs()
                .AddInline(
                    string.Format(
                        "new BootstrapTextareaBox({{id:'{0}', formId:'{1}', maxLength:{2}, warnOnLeave:{3}}});",
                        tabProps.Id,
                        this.Id,
                        Conversion.ConvertToInt(tabProps.MaxLength, 0),
                        this.GetJsBool(tabProps.WarnOnLeave)));
            

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

            tabProp.Name = this.GetExpressionAsName(expression);
            tabProp.Id = this.GetIdFromName(tabProp);
            tabProp.Messages = this.GetMessagesForProperty(expression, this.Messages);
            tabProp.Value = value;

            return this.TextareaBox(tabProp);
        }

        public MvcHtmlString Button(Button bProps)
        {
            this.RegisterControl(bProps);

            if (!this.Cruncher.HeaderJs().HasItem("BootstrapButton_js"))
            {
                this.Cruncher.HeaderJs()
                          .AddInline(
                              Resources.BootstrapButton_js,
                              name: "BootstrapButton_js",
                              order: StoreItem.OrderOptions.Middle);
            }

            this.Cruncher.HeaderJs()
                .AddInline(
                    string.Format(
                        "new BootstrapButton({{id:'{0}', "
                        + "formId:'{1}', "
                        + "url:'{2}', " 
                        + "targetNewWindow:{3}, " 
                        + "launchLoadingOnClick:{4}, " 
                        + "loadingScreenTitle:'{5}', " 
                        + "confirmOnClick:{6}, " 
                        + "confirmTitle:'{7}' }});",
                        bProps.Id,
                        this.Id,
                        bProps.Url ?? string.Empty,
                        this.GetJsBool(bProps.TargetNewWindow),
                        this.GetJsBool(bProps.LaunchLoadingOnclick),
                        bProps.LoadingScreenTitle ?? string.Empty,
                        this.GetJsBool(bProps.ConfirmOnClick),
                        bProps.ConfirmTitle ?? string.Empty));

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

            bProps.Name = this.GetExpressionAsName(expression);
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

            var output = string.Format("<div class=\"bs-upload-for\" id=\"{0}_container\">", ubProps.Id);

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

            if (!this.Cruncher.HeaderJs().HasItem("BootstrapUpload_js"))
            {
                this.Cruncher.HeaderJs()
                          .AddInline(
                              Resources.BootstrapUpload_js,
                              name: "BootstrapUpload_js",
                              order: StoreItem.OrderOptions.Middle);

                this.Cruncher.HeaderCss()
                          .AddInline(
                              Resources.BootstrapUpload_less,
                              name: "BootstrapUpload_less",
                              less: true,
                              order: StoreItem.OrderOptions.Middle);
            }

            this.Cruncher.HeaderJs()
                    .AddInline(
                        string.Format(
                            "new BootstrapUpload({{id:'{0}', formId:'{1}', warnOnLeave: {2}}});",
                            ubProps.Id,
                            this.Id,
                            this.GetJsBool(ubProps.WarnOnLeave)));

            return MvcHtmlString.Create(output);
        }

        public MvcHtmlString UploadFor(Expression<Func<TModel, UploadModel>> expression, UploadBox ubProps)
        {
            var value = this.GetExpressionValue(expression);

            ubProps.Name = this.GetExpressionAsName(expression);
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

            if (!this.Cruncher.HeaderJs().HasItem("BootstrapCheckBoxList_js"))
            {
                this.Cruncher.HeaderJs()
                          .AddInline(
                              Resources.BootstrapCheckBoxList_js,
                              name: "BootstrapCheckBoxList_js",
                              order: StoreItem.OrderOptions.Middle);
            }

            this.Cruncher.HeaderJs()
                    .AddInline(
                        string.Format(
                            "new BootstrapCheckBoxList({{id:'{0}', formId:'{1}', name:'{2}', warnOnLeave:{3} }});",
                            cblProps.Id,
                            this.Id,
                            cblProps.Name,
                            this.GetJsBool(cblProps.WarnOnLeave)));

            return MvcHtmlString.Create(output);
        }

        public MvcHtmlString CheckBoxListFor<TOptionKey, TOptionValue>(
            Expression<Func<TModel, List<TOptionKey>>> expression,
            CheckBoxList<TOptionKey, TOptionValue> cblProps)
        {
            var value = this.GetExpressionValue(expression);

            cblProps.Name = this.GetExpressionAsName(expression);
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

            if (!this.Cruncher.HeaderJs().HasItem("BootstrapCheckBox_js"))
            {
                this.Cruncher.HeaderJs()
                          .AddInline(
                              Resources.BootstrapCheckBox_js,
                              name: "BootstrapCheckBox_js",
                              order: StoreItem.OrderOptions.Middle);
            }

            this.Cruncher.HeaderJs()
                    .AddInline(
                        string.Format(
                            "new BootstrapCheckBox({{id:'{0}', formId:'{1}', warnOnLeave:{2}}});",
                            cbProps.Id,
                            this.Id,
                            this.GetJsBool(cbProps.WarnOnLeave)));

            return MvcHtmlString.Create(output);
        }

        public MvcHtmlString CheckBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, CheckBox cbProps)
        {
            var value = Conversion.ConvertToBoolean(this.GetExpressionValue(expression));

            cbProps.Name = this.GetExpressionAsName(expression);
            cbProps.Id = this.GetIdFromName(cbProps);
            cbProps.Messages = this.GetMessagesForProperty(expression, this.Messages);
            cbProps.Checked = value;

            return this.CheckBox(cbProps);
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

            if (!this.Cruncher.HeaderJs().HasItem("BootstrapSelectList_js"))
            {
                this.Cruncher.HeaderJs()
                          .AddInline(
                              Resources.BootstrapSelectList_js,
                              name: "BootstrapSelectList_js",
                              order: StoreItem.OrderOptions.Middle);
            }

            this.Cruncher.HeaderJs()
                    .AddInline(
                        string.Format(
                            "new BootstrapSelectList({{id:'{0}', formId:'{1}', warnOnLeave:{2}}});",
                            sProps.Id,
                            this.Id,
                            this.GetJsBool(sProps.WarnOnLeave)));

            return MvcHtmlString.Create(output);
        }

        public MvcHtmlString SelectListFor<TOptionKey, TOptionValue>(
            Expression<Func<TModel, TOptionKey>> expression,
            SelectList<TOptionKey, TOptionValue> sProps)
        {
            var value = this.GetExpressionValue(expression);

            sProps.Name = this.GetExpressionAsName(expression);
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

            sProps.Name = this.GetExpressionAsName(expression);
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

            if (!this.Cruncher.HeaderJs().HasItem("BootstrapRadioList_js"))
            {
                this.Cruncher.HeaderJs()
                          .AddInline(
                              Resources.BootstrapRadioList_js,
                              name: "BootstrapRadioList_js",
                              order: StoreItem.OrderOptions.Middle);
            }

            this.Cruncher.HeaderJs()
                    .AddInline(
                        string.Format(
                            "new BootstrapRadioList({{id:'{0}', formId:'{1}', name:'{2}', warnOnLeave:{3}}});",
                            rlProps.Id,
                            this.Id,
                            rlProps.Name,
                            this.GetJsBool(rlProps.WarnOnLeave)));

            return MvcHtmlString.Create(output);
        }

        public MvcHtmlString RadioListFor<TOptionKey, TOptionValue>(
            Expression<Func<TModel, TOptionKey>> expression,
            RadioList<TOptionKey, TOptionValue> rlProps)
        {
            var value = this.GetExpressionValue(expression);

            rlProps.Name = this.GetExpressionAsName(expression);
            rlProps.Id = this.GetIdFromName(rlProps);
            rlProps.Messages = this.GetMessagesForProperty(expression, this.Messages);
            rlProps.SelectedOption = value;

            return this.RadioList(rlProps);
        }

        public MvcHtmlString DateTimePicker(DateTimePicker dtpProps)
        {
            this.RegisterControl(dtpProps);

            if (!this.Cruncher.HeaderJs().HasItem("BootstrapDateTimePicker_js"))
            {
                this.Cruncher.HeaderJs()
                          .AddInline(
                              Resources.BootstrapDateTimePicker_js,
                              name: "BootstrapDateTimePicker_js",
                              order: StoreItem.OrderOptions.Middle);
                this.Cruncher.HeaderCss()
                          .AddInline(
                              Resources.BootstrapDateTimePicker_less, name: "BootstrapDateTimePicker_less");
            }

            var format = string.Empty;
            var pickDate = false;
            var pickTime = false;
            var pickSeconds = false;

            switch (dtpProps.DateTimeFormat)
            {
                case DateTimeFormat.DateAndTime:
                    format = "dd/MM/yyyy hh:mm:ss";
                    pickDate = true;
                    pickTime = true;
                    pickSeconds = true;
                    break;
                case DateTimeFormat.Date:
                    format = "dd/MM/yyyy";
                    pickDate = true;
                    break;
                case DateTimeFormat.Time:
                    format = "hh:mm:ss";
                    pickTime = true;
                    pickSeconds = true;
                    break;
                case DateTimeFormat.TimeNoSeconds:
                    format = "hh:mm";
                    pickTime = true;
                    break;
                case DateTimeFormat.DateTimeNoSeconds:
                    format = "dd/MM/yyyy hh:mm";
                    pickDate = true;
                    pickTime = true;
                    break;
            }

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

            var result = "<div id=\"" + dtpProps.Id + "_helper\" class=\"bootstrapDateTimePicker\">\n"
                         + "<div class=\"input-group\">\n" + "<input class=\"form-control date-input "
                         + (dtpProps.TextSize == null ? string.Empty : FormatInputTextSize(dtpProps.TextSize)) + "\" id=\"" + dtpProps.Id
                         + "\" name=\"" + dtpProps.Name + "\" value=\"" + (dtpProps.Value == null ? string.Empty : ((DateTime)dtpProps.Value).ToString(format))
                         + "\" title=\"Entry format is " + format + "\" type=\"text\" "
                         + (this.IsControlEnabled(dtpProps.Enabled) ? string.Empty : "readonly=\"readonly\"") + "/>\n";

            if (this.IsControlEnabled(dtpProps.Enabled))
            {
                result += "<span class=\"input-group-btn\">\n"
                          + "<button class=\"btn btn-" + level + " date-select\" type=\"button\">\n"
                          + "<i class=\"glyphicon " + (pickTime && !pickDate ? "glyphicon-time" : "glyphicon-calendar") + "\">\n</i>"
                          + "<button class=\"btn btn-" + level + " date-delete\" type=\"button\">\n"
                          + "<i class=\"glyphicon glyphicon-remove-circle\">"
                          + "</i>\n</button>\n</span>";
            }

            result += "\n</div>\n</div>\n";

            result = this.MakeFormGroup(result, dtpProps);

            this.Cruncher.HeaderJs().AddInline(
                string.Format(
                "new BootstrapDateTimePicker("
                + "{{id:'{0}', formId:'{1}', pickDate:{2}, pickTime:{3}, pickSeconds:{4}, warnOnLeave:{5}, popoverPlacement:'{6}', format:'{7}', timezone:'{8}'}});",
                dtpProps.Id,
                this.Id,
                pickDate.ToString(CultureInfo.InvariantCulture).ToLower(),
                pickTime.ToString(CultureInfo.InvariantCulture).ToLower(),
                pickSeconds.ToString(CultureInfo.InvariantCulture).ToLower(),
                this.GetJsBool(dtpProps.WarnOnLeave),
                this.PopoverPlacementToString(dtpProps.PopoverPlacement),
                format,
                dtpProps.Timezone));

            return MvcHtmlString.Create(result);
        }

        public MvcHtmlString DateTimePickerFor<TValue>(
            Expression<Func<TModel, TValue>> expression,
            DateTimePicker dtpProps)
        {
            var value = Conversion.ConvertToDateTime(this.GetExpressionValue(expression));

            dtpProps.Name = this.GetExpressionAsName(expression);
            dtpProps.Id = this.GetIdFromName(dtpProps);
            dtpProps.Messages = this.GetMessagesForProperty(expression, this.Messages);
            dtpProps.Value = value;

            return this.DateTimePicker(dtpProps);
        }

        public MvcHtmlString SearchBox(SearchBox sbProps)
        {
            this.RegisterControl(sbProps);

            if (!this.Cruncher.HeaderJs().HasItem("BootstrapSearchBox_js"))
            {
                this.Cruncher.HeaderJs()
                          .AddInline(
                              Resources.BootstrapSearchBox_js,
                              name: "BootstrapSearchBox_js",
                              order: StoreItem.OrderOptions.Middle);
                this.Cruncher.HeaderCss().AddInline(Resources.BootstrapSearchBox_less, name: "BootstrapSearchBox_less", less: true);
            }

            var result = string.Format(
                "<input type=\"hidden\" id=\"{0}\" name=\"{1}\" value=\"{2}\" />\n", sbProps.Id, sbProps.Name, sbProps.Value);
            result += string.Format("<div class=\"bs-search-box\" id=\"{0}_bs_search_box\">\n", sbProps.Id);

            result += string.Format(
                "<input type=\"text\" id=\"{0}_search\" name=\"{1}.search\" class=\"form-control {2}\" value=\"{3}\" {4} autocomplete=\"off\" />\n",
                sbProps.Id,
                sbProps.Name,
                sbProps.TextSize == null ? string.Empty : FormatInputTextSize(sbProps.TextSize),
                sbProps.Text,
                this.IsControlEnabled(sbProps.Enabled) ? string.Empty : "readonly=\"readonly\"");

            result += "</div>\n";

            result = this.MakeFormGroup(result, sbProps);

            this.Cruncher.HeaderJs()
                      .AddInline(
                          string.Format(
                              "new BootstrapSearchBox("
                              + "{{id:'{0}', formId:'{1}', name: '{2}', selectEvent: {3}, dataSourceUrl: '{4}', "
                              + "searchValueField: '{5}', searchTextField: '{6}', selectedTextField: '{7}', scrollHeight: {8}, "
                              + "popoverPlacement: '{9}', popoutWidth: {10} }});",
                              sbProps.Id,
                              this.Id,
                              sbProps.Name,
                              string.IsNullOrEmpty(sbProps.SelectEvent) ? "null" : sbProps.SelectEvent,
                              sbProps.DataSourceUrl,
                              sbProps.SearchValueField,
                              sbProps.SearchTextField,
                              sbProps.SelectedTextField,
                              sbProps.ScrollHeight,
                              this.PopoverPlacementToString(sbProps.PopoverPlacement),
                              sbProps.PopoutWidth == null ? "null" : sbProps.PopoutWidth.ToString()));

            return MvcHtmlString.Create(result);
        }

        public MvcHtmlString SearchBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, SearchBox sbProps)
        {
            sbProps.Name = this.GetExpressionAsName(expression);
            sbProps.Id = this.GetIdFromName(sbProps);
            sbProps.Messages = this.GetMessagesForProperty(expression, this.Messages);

            return this.SearchBox(sbProps);
        }

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

            if (this.IsTitleShowing(cBase.ShowTitle))
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

        private string MakeInputGroup(string contents, FormControl cbase) 
        {
            return string.Format("<div class=\"input-group\">{0}</div>\n", contents);
        }

        private string MakeTextBox(TextBox tbProps, bool incJs)
        {
            //// first lets add our javascript core and control instance
            if (!this.Cruncher.HeaderJs().HasItem("BootstrapTextBox_js"))
            {
                this.Cruncher.HeaderJs()
                          .AddInline(
                              Resources.BootstrapTextBox_js,
                              name: "BootstrapTextBox_js",
                              order: StoreItem.OrderOptions.Middle);
            }

            if (incJs)
            {
                this.Cruncher.HeaderJs()
                    .AddInline(
                        string.Format(
                            "var {0}_BootstrapTextbox = new BootstrapTextbox({{id:'{0}', formId:'{1}', maxLength:{2}, warnOnLeave:{3}}});",
                            tbProps.Id,
                            this.Id,
                            Conversion.ConvertToInt(tbProps.MaxLength, 0),
                            this.GetJsBool(tbProps.WarnOnLeave)));
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

        private void IncludeNumericJs()
        {
            if (!this.Cruncher.HeaderJs().HasItem("BootstrapNumericBox_js"))
            {
                this.Cruncher.HeaderJs()
                          .AddInline(
                              Resources.BootstrapNumericBox_js,
                              name: "BootstrapNumericBox_js",
                              order: StoreItem.OrderOptions.Middle);
            }
        }

        /// <summary>
        /// Determine a control should be disabled based on the provided
        /// parameter and class property ControlsEnabled. If disabled
        /// is then class property determines the outcome. 
        /// </summary>
        /// <param name="disabled">The value passed directly to the method</param>
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

        private bool IsTitleShowing(bool? showTitle)
        {
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
        private List<Message> GetMessagesForProperty<TValue>(Expression<Func<TModel, TValue>> expression, IEnumerable<Message> messages)
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
    }
}