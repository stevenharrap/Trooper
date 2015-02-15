//--------------------------------------------------------------------------------------
// <copyright file="Table.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Bootstrap
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using Trooper.Properties;
    using Trooper.Ui.Interface.Mvc.Cruncher;
    using Trooper.Ui.Mvc.Bootstrap.TableClasses;
    using Trooper.Ui.Mvc.Bootstrap.TableClasses.Footer;
    using Trooper.Ui.Mvc.Cruncher;
    using Trooper.Utility;

    /// <summary>
    /// This class creates paged and sorted grid with columns in the form described by the 
    /// Bootstrap library. The class uses WebGrid as a base but then extends its so that grid
    /// can be used in a form (allows you to post your form and keep paging and sorting) 
    /// and so that rows can be highlighted.
    /// </summary>
    /// <remarks>
    /// The use of WebGrid could eventually be removed if I can figure out the WebGrid works. 
    /// </remarks>
    /// <typeparam name="TModel">
    /// The model type in your view
    /// </typeparam>
    /// <typeparam name="TKey">
    /// The data type of the key the identifies each row.
    /// </typeparam>
    public class Table<TModel, TKey> : Html<TModel>
        where TKey : class, IEquatable<TKey>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Table{TModel,TKey}"/> class.
        /// </summary>
        /// <param name="id">
        /// The id of the table.
        /// </param>
        /// <param name="source">
        /// The data source for the table.
        /// </param>
        /// <param name="htmlHelper">
        /// The html helper.
        /// </param>
        public Table(string id, IEnumerable<dynamic> source, HtmlHelper<TModel> htmlHelper)
            : base(htmlHelper)
        {
            this.Id = id;
            this.Source = source;
            this.Visible = true;
            this.Striped = false;
            this.Bordered = false;
            this.Hover = false;
            this.Condensed = false;
            this.TableActionParameters = null;
            this.RowSelectionMode = TableRowSelectionModes.None;
            this.RowsPerPage = 15;
            this.CanPage = true;
            this.CanSort = true;
            this.Caption = null;
            this.Columns = null;
        }

        /// <summary>
        /// Gets or sets the id of the table.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the keys for rows that should be highlighted in some manner.
        /// The properties on the RowFormat allow your specify the style of highlight.
        /// </summary>
        public List<RowFormat<TKey>> RowFormating { get; set; }

        /// <summary>
        /// Gets or sets the column that should be sorted by default.
        /// </summary>
        public string DefaultSort { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the table can be sorted.
        /// </summary>
        public bool CanSort { get; set; }

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        public IEnumerable<dynamic> Source { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the table should appear striped.
        /// </summary>
        public bool Striped { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the rows should be bordered.
        /// </summary>
        public bool Bordered { get; set; }

        /// <summary>
        /// Gets or sets the number of rows per page.
        /// </summary>
        public int RowsPerPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the table should allow paging. True by default.
        /// </summary>
        public bool CanPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether rows should be light up on mouse hover.
        /// </summary>
        public bool Hover { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the table should condensed to save space.
        /// </summary>
        public bool Condensed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the table should be visible.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the footer rows that appear above the paging options. This is just a collection
        /// of rows and columns - there is no relationship to the data types or data source that has been 
        /// provided to the table.
        /// </summary>
        public List<Row> FooterRows { get; set; }

        /// <summary>
        /// Gets or sets the table action parameters (new { name1 = value, name2 = value }). When in a form the clicking
        /// of a page or column control should cause the form to submit and include these values as hidden values. When not
        /// in a form the parameters should be attached to the HREF in the controls. When in a form the HREF parameters will
        /// be attached to the action of the form and then form will be submitted.
        /// The table will be re-rendered and the GET parameters in the form will tell the table how to sort and page.
        /// </summary>
        public object TableActionParameters { get; set; }

        /// <summary>
        /// Gets or sets the row selection mode. Allows multiple row selection.
        /// </summary>
        public TableRowSelectionModes RowSelectionMode { get; set; }

        /// <summary>
        /// Gets or sets the caption for the table.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the columns of the table.
        /// </summary>
        public List<WebGridColumn> Columns { get; set; }

        /// <summary>
        /// Renders the table.
        /// </summary>
        /// <returns>
        /// The <see cref="IHtmlString"/>.
        /// </returns>
        public IHtmlString Render()
        {
            var webGrid = new WebGrid(
                source: this.Source,
                defaultSort: this.DefaultSort,
                fieldNamePrefix: this.Id + "_",
                rowsPerPage: this.RowsPerPage, 
                canSort: this.CanSort,
                canPage: this.CanPage);

            this.InitTable();

            if (!this.Visible)
            {
                return new HtmlString(string.Empty);
            }

            var rawTable = this.CreateTable(webGrid);
            
            var domTable = XDocument.Parse(rawTable);

            domTable.XPathSelectElements("/table/thead/tr/th").First().Remove();
            domTable.XPathSelectElements("/table/thead/tr/th").First().Remove();

            this.DoRowHighlighting(domTable);
            this.DoHeaderLinkCorrection(domTable);
            this.DoHeaderAltTitles(domTable);
            this.DoPagingFooterLinkCorrection(domTable);
            this.InsertFoorterRows(domTable);

            return new HtmlString(domTable.ToString());
        }

        /// <summary>
        /// Initiates the JavaScript and CSS for the table. Even if visible is false this will still occur.
        /// </summary>
        private void InitTable()
        {
            var cruncher = new Cruncher(this.HtmlHelper);

            if (!cruncher.HasJsItem("BootstrapTable_js"))
            {
                cruncher.AddJsInline(
                              Resources.BootstrapTable_js).Set(
                              name: "BootstrapTable_js",
                              order: OrderOptions.Middle);

                cruncher.AddLessInline(
                              Resources.BootstrapTable_less).Set(
                              name: "BootstrapTable_less",
                              order: OrderOptions.Middle);
            }

            cruncher.AddJsInline(
                          string.Format(
                              "var bootstrapTable_{0} = new BootstrapTable({{ id: '{0}', rowSelectionMode: '{1}' }});",
                              this.Id,
                              this.RowSelectionMode));
        }

        /// <summary>
        /// The creates table as a string for further formatting. Uses WebGrid and adds extra columns
        /// at this point to record key values and other formatting information for each row. These 
        /// are disposed of later on.
        /// </summary>
        /// <param name="webGrid">
        /// The web grid.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string CreateTable(WebGrid webGrid)
        {
            foreach (var wgc in this.Columns)
            {
                if (wgc is Column)
                {
                    var c = wgc as Column;

                    if (c.NoWrap)
                    {
                        c.Style = string.Format(
                            "{0}{1}no-wrap",
                            c.Style,
                            string.IsNullOrEmpty(c.Style) ? string.Empty : " ");
                    }
                }
            }

            var tableStyle = string.Format(
                "trooper table {0} {1} {2} {3}",
                this.Striped ? "table-striped" : null,
                this.Bordered ? "table-bordered" : null,
                this.Hover ? "table-hover" : null,
                this.Condensed ? "table-condensed" : null);

            var jss = new JavaScriptSerializer();

            if (this.Columns == null)
            {
                throw new Exception("You must have some columns in your table!");
            }

            var attrs = new { id = this.Id };
            
            var hightlightColumn = webGrid.Column(
                canSort: false,
                style: "bs-grid-row-type",
                format: item =>
                {
                    TKey keyValue = item.Value;
                    var classes = new List<string>();

                    if (this.RowFormating != null && this.RowFormating.Any(r => r.Key.Equals(keyValue) && r.Highlighted == RowHighlight.Success))
                    {
                        classes.Add("success");
                    }
                    else if (this.RowFormating != null && this.RowFormating.Any(r => r.Key.Equals(keyValue) && r.Highlighted == RowHighlight.Danger))
                    {
                        classes.Add("danger");
                    }
                    else if (this.RowFormating != null && this.RowFormating.Any(r => r.Key.Equals(keyValue) && r.Highlighted == RowHighlight.Warning))
                    {
                        classes.Add("warning");
                    }
                    else if (this.RowFormating != null && this.RowFormating.Any(r => r.Key.Equals(keyValue) && r.Highlighted == RowHighlight.Active))
                    {
                        classes.Add("active");
                    }
                    else if (this.RowFormating != null && this.RowFormating.Any(r => r.Key.Equals(keyValue) && r.Highlighted == RowHighlight.Info))
                    {
                        classes.Add("info");
                    }

                    if (this.RowFormating != null && this.RowFormating.Any(r => r.Key.Equals(keyValue) && r.Bold))
                    {
                        classes.Add("bold");
                    }

                    // Changing Text Colour
                    if (this.RowFormating != null && this.RowFormating.Any(r => r.Key.Equals(keyValue) && r.RowTextHighlightStyle == RowTextHighlight.Success))
                    {
                        classes.Add("text-success");
                    }
                    else if (this.RowFormating != null && this.RowFormating.Any(r => r.Key.Equals(keyValue) && r.RowTextHighlightStyle == RowTextHighlight.Danger))
                    {
                        classes.Add("text-danger");
                    }
                    else if (this.RowFormating != null && this.RowFormating.Any(r => r.Key.Equals(keyValue) && r.RowTextHighlightStyle == RowTextHighlight.Warning))
                    {
                        classes.Add("text-warning");
                    }
                    else if (this.RowFormating != null && this.RowFormating.Any(r => r.Key.Equals(keyValue) && r.RowTextHighlightStyle == RowTextHighlight.Active))
                    {
                        classes.Add("text-active");
                    }
                    else if (this.RowFormating != null && this.RowFormating.Any(r => r.Key.Equals(keyValue) && r.RowTextHighlightStyle == RowTextHighlight.Info))
                    {
                        classes.Add("text-info");
                    }

                    if (this.RowFormating != null)
                    {
                        var ruleUnderStyle =
                            this.RowFormating.FirstOrDefault(
                                r => r.Key.Equals(keyValue) && r.RuleUnderStyle != RuleStyle.Default);

                        var ruleOverStyle =
                            this.RowFormating.FirstOrDefault(
                                r => r.Key.Equals(keyValue) && r.RuleOverStyle != RuleStyle.Default);

                        if (ruleUnderStyle != null)
                        {
                            classes.Add("rule-under");
                            classes.Add(string.Format("rule-under-{0}", ruleUnderStyle.RuleUnderStyle.ToString().ToLower()));
                        }

                        if (ruleOverStyle != null)
                        {
                            classes.Add("rule-over");
                            classes.Add(string.Format("rule-over-{0}", ruleOverStyle.RuleOverStyle.ToString().ToLower()));
                        }
                    }

                    return string.Join(" ", classes);
                });

            var dataValueColumn = webGrid.Column(
                canSort: false,
                format: item =>
                    {
                        //// This gets the keys from the item and creates a proper TKey instance.
                        //// Otherwize we end up with all the properties from the item in the data-item.
                        var keyValue = new TKey();
                        foreach (var prop in keyValue.GetType().GetProperties())
                        {
                            var value = item[prop.Name];
                            prop.SetValue(keyValue, value);
                        }

                        return jss.Serialize(keyValue);
                    });

            this.Columns.Insert(0, hightlightColumn);
            this.Columns.Insert(1, dataValueColumn);

            return webGrid.GetHtml(
                tableStyle: tableStyle, caption: this.Caption, columns: this.Columns, htmlAttributes: attrs).ToString();
        }

        /// <summary>
        /// If there are any rows that should be highlighted then add styles.
        /// Remove the column that indicates this.
        /// </summary>
        /// <param name="domTable">
        /// The dom table.
        /// </param>
        private void DoRowHighlighting(XDocument domTable)
        {
            foreach (var tr in domTable.XPathSelectElements("/table/tbody/tr"))
            {
                var tds = tr.Elements("td").ToList();

                if (tds.Count() < 2)
                {
                    continue;
                }

                var hightlightTd = tds.ElementAt(0);
                var dataValueTd = tds.ElementAt(1);

                if (!string.IsNullOrEmpty(hightlightTd.Value))
                {
                    tr.SetAttributeValue("class", hightlightTd.Value);
                }

                tr.SetAttributeValue("data-item", dataValueTd.Value);

                hightlightTd.Remove();
                dataValueTd.Remove();
            }
        }

        /// <summary>
        /// If there are any TableActionParameters then the table column links will be updated with those parameters. 
        /// In a form situation these parameters are attached to the form action and the form submitted. 
        /// </summary>
        /// <param name="domTable">
        /// The dom table.
        /// </param>
        private void DoHeaderLinkCorrection(XDocument domTable)
        {
            var links = domTable.XPathSelectElements("/table/thead/tr/th/a");

            foreach (var link in links)
            {
                var hrefAttr = link.Attribute("href");

                if (hrefAttr == null)
                {
                    continue;
                }

                hrefAttr.Value = this.AdjustHref(hrefAttr.Value);
            }
        }

        /// <summary>
        /// Goes through the columns looking for any with alternate titles and records
        /// them into an attribute called "alt-headers" on the TH. JavaScript
        /// will show the appropriate header client side.
        /// </summary>
        /// <param name="domTable">The table XML object</param>
        private void DoHeaderAltTitles(XDocument domTable)
        {
            var links = domTable.XPathSelectElements("/table/thead/tr/th").ToList();

            for (var i = 2; i < this.Columns.Count; i++)
            {
                if (this.Columns[i] is Column)
                {
                    var c = this.Columns[i] as Column;
                    
                    if (c.HasSpecificHeaders)
                    {
                        var altHeaders =
                            new
                                {
                                    xs = c.HeaderExtraSmall ?? string.Empty,
                                    sm = c.HeaderSmall ?? string.Empty,
                                    md = c.HeaderMedium ?? string.Empty,
                                    lg = c.Header ?? string.Empty,
                                    pr = c.HeaderPrint ?? string.Empty
                                };

                        links[i - 2].SetAttributeValue("alt-headers", Json.Encode(altHeaders));
                    }
                }
            }
        }

        /// <summary>
        /// If there are any TableActionParameters then the table page links will be updated with those parameters. 
        /// In a form situation these parameters are attached to the form action and the form submitted. 
        /// The paging is also replaced with a UL structure.
        /// </summary>
        /// <param name="domTable">
        /// The dom table.
        /// </param>
        private void DoPagingFooterLinkCorrection(XDocument domTable)
        {
            var td = domTable.XPathSelectElement("/table/tfoot/tr/td");

            if (td == null)
            {
                return;
            }
            
            td.SetAttributeValue("colspan", this.Columns.Count - 2);

            var pages = new List<Page>();

            foreach (var node in td.Nodes())
            {
                var value = string.Empty;
                var href = string.Empty;

                switch (node.NodeType)
                {
                    case XmlNodeType.Element:
                        {
                            var element = node as XElement;
                            if (element == null)
                            {
                                continue;
                            }

                            var hrefAttr = element.Attribute("href");

                            if (hrefAttr == null)
                            {
                                continue;
                            }

                            value = element.Value;
                            href = hrefAttr.Value;
                        }

                        break;

                    case XmlNodeType.Text:
                        {
                            var element = node as XText;
                            if (element == null)
                            {
                                continue;
                            }

                            value = element.Value;
                        }

                        break;
                }

                pages.Add(new Page { Value = value, Href = this.AdjustHref(href) });
            }
            
            td.RemoveNodes();

            var rowDiv = new XElement("div");
            rowDiv.SetAttributeValue("class", "paging");

            var cell1Div = new XElement("div");
            cell1Div.SetAttributeValue("class", "left");
            
            var ul = new XElement("ul");
            ul.SetAttributeValue("class", "pagination");

            foreach (var p in pages)
            {
                var li = new XElement("li");
                var a = new XElement("a");

                if (string.IsNullOrEmpty(p.Href))
                {
                    li.SetAttributeValue("class", "disabled");
                }
                else
                {
                    a.SetAttributeValue("href", p.Href);
                }

                a.Value = p.Value;
                li.Add(a);
                ul.Add(li);
            }
            
            cell1Div.Add(ul);
            rowDiv.Add(cell1Div);

            var cell2Div = new XElement("div");
            cell2Div.SetAttributeValue("class", "right");
            cell2Div.Value = string.Format("{0} items", this.Source.Count());
            rowDiv.Add(cell2Div);

            td.Add(rowDiv);
        }

        /// <summary>
        /// The insert footer rows into the table above the paging options.
        /// </summary>
        /// <param name="domTable">
        /// The dom table.
        /// </param>
        private void InsertFoorterRows(XDocument domTable)
        {
            XElement tableFoot;

            if (this.FooterRows == null || !this.FooterRows.Any())
            {
                return;
            }

            var hasFooter = domTable.XPathSelectElement("/table/tfoot") != null;

            if (!hasFooter)
            {
                tableFoot = new XElement("tfoot");
                var tableHead = domTable.XPathSelectElement("/table/thead");
                tableHead.AddAfterSelf(tableFoot);
            }

            tableFoot = domTable.XPathSelectElement("/table/tfoot");
            var pagingTr = domTable.XPathSelectElement("/table/tfoot/tr");

            foreach (var r in this.FooterRows)
            {
                var tr = new XElement("tr");

                tr.SetAttributeValue("class", "footer");

                if (!string.IsNullOrEmpty(r.Class))
                {
                    tr.SetAttributeValue("class", "footer " + r.Class);
                }

                foreach (var c in r.Cells)
                {
                    var td = new XElement("td");

                    if (c.ColSpan > 1)
                    {
                        td.SetAttributeValue("colspan", c.ColSpan);
                    }

                    if (!string.IsNullOrEmpty(c.Class))
                    {
                        td.SetAttributeValue("class", c.Class);
                    }

                    if (!string.IsNullOrEmpty(c.Content))
                    {
                        switch (c.ContentType)
                        {
                            case Cell.ContentTypes.Html:
                                var html = XElement.Parse(c.Content);
                                td.Add(html);
                                break;

                            case Cell.ContentTypes.Text:
                                td.SetValue(c.Content);

                                break;
                        }
                    }

                    tr.Add(td);
                }

                if (pagingTr == null)
                {
                    tableFoot.Add(tr);
                }
                else
                {
                    pagingTr.AddBeforeSelf(tr);
                }
            }
        }

        /// <summary>
        /// The adjust HREF so that it includes the TableActionParameters.
        /// </summary>
        /// <param name="href">
        /// The HREF.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string AdjustHref(string href)
        {
            if (this.TableActionParameters == null)
            {
                return href;
            }

            var parts = href.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
            {
                return href;
            }

            var queryString = HttpUtility.ParseQueryString(parts[1]);

            foreach (PropertyDescriptor p in TypeDescriptor.GetProperties(this.TableActionParameters))
            {
                queryString.Set(p.Name, Conversion.ConvertToString(p.GetValue(this.TableActionParameters)));
            }

            return string.Format("{0}?{1}", parts[0], queryString);
        }
    }
}
