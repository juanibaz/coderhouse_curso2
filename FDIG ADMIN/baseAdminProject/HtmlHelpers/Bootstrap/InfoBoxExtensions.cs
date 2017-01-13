using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace Nimok.Mvc.Bootstrap.Extensions
{
    public enum GlypType
    {
        Icon, Progress, Chart
    }
    public enum InfoBoxColor
    {
        [Description("infobox-purple")]
        Purple,
        [Description("infobox-purple2")]
        Purple2,
        [Description("infobox-pink")]
        Pink,
        [Description("infobox-blue")]
        Blue,
        [Description("infobox-blue2")]
        Blue2,
        [Description("infobox-blue3")]
        Blue3,
        [Description("infobox-red")]
        Red,
        [Description("infobox-brown")]
        Brown,
        [Description("infobox-wood")]
        Wood,
        [Description("infobox-light-brown")]
        LightBrown,
        [Description("infobox-orange")]
        Orange,
        [Description("infobox-orange2")]
        Orange2,
        [Description("infobox-green")]
        Green,
        [Description("infobox-green2")]
        Green2,
        [Description("infobox-grey")]
        Grey,
        [Description("infobox-black")]
        Black,
        [Description("infobox-dark")]
        Dark
    }

    public enum ContentType
    {
        Number, Text
    }
    public enum StatusType
    {
        None,
        [Description("badge badge-success")]
        BadgeUpward,
        [Description("badge badge-important")]
        BadgeDownward,
        [Description("stat stat-success")]
        StatusUpward,
        [Description("stat stat-important")]
        StatusDownward
    }

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }

    public enum SparklineType
    {
        Line, Bar, Tristate, Discrete, Bullet, Pie, Box
    }

    public class SparklineOptions
    {
        public SparklineType Type { get; set; }
        //public string Color { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
    }

    public class InfoBox<TModel> : BootstrapControl<TModel, InfoBox<TModel>>
    //public class Label<TModel> : BootstrapControl<TModel, Label<TModel>>
    {
        private readonly HtmlHelper htmlHelper;
        private InfoBoxColor infoBoxColor;
        private GlypType glyphType;

        // progress glyph type
        private int progressPercent;
        //private int progressDataSize;

        // icon glyph type
        private string iconClass;

        // chart glyph type
        private int[] chartSerieValues;

        // data-content
        private ContentType dataContentType;
        private string dataText;
        private string dataContent;

        // data-stat
        private int statusNumber;
        private StatusType? statusType;
        private bool statusArrowDown;

        // style
        private bool darkStyle;
        private string cssPull;

        private string actionUrl;

        // sparkline
        private SparklineOptions sparklineOptions;

        /// <summary>
        /// Html tag Id
        /// </summary>
        private string id;

        public InfoBox(HtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
            this.infoBoxColor = InfoBoxColor.Blue;
            this.statusType = StatusType.None;
            
            this.sparklineOptions = new SparklineOptions() { Type = SparklineType.Bar };
        }

        public InfoBox<TModel> WithColor(InfoBoxColor infoBoxColor)
        {
            this.infoBoxColor = infoBoxColor;
            return this;
        }

        public InfoBox<TModel> GlyphIcon(string iconClass)
        {
            this.glyphType = GlypType.Icon;
            this.iconClass = iconClass;
            return this;
        }

        public InfoBox<TModel> GlyphProgress(int percent)
        {
            /*
             *<div class="infobox-progress">
				 <div class="easy-pie-chart percentage" data-percent="12" data-size="46">
				   <span class="percent">12</span>%
				 </div>
			  </div>
             */
            this.glyphType = GlypType.Progress;
            this.progressPercent = percent;
            //this.progressDataSize = dataSize;
            return this;
        }

        public InfoBox<TModel> GlyphChart(int[] serieValues)
        {
            /*
	            <div class="infobox-chart">
				    <span class="sparkline" data-values="373,366,268,298,300,341,815"></span>
                </div>
             */
            this.GlyphChart(new SparklineOptions { Type = SparklineType.Bar }, serieValues);
            return this;
        }

        public InfoBox<TModel> GlyphChart(SparklineOptions options, int[] serieValues)
        {
            this.glyphType = GlypType.Chart;
            this.chartSerieValues = serieValues;
            this.sparklineOptions = options;
            return this;
        }

        public InfoBox<TModel> WithContent(ContentType contentType, string dataText, string content)
        {
            this.dataContentType = contentType;
            this.dataText = dataText;
            this.dataContent = content;
            return this;
        }

        public InfoBox<TModel> WithContent(string dataText, string content)
        {
            this.dataContentType = ContentType.Text;
            this.dataText = dataText;
            this.dataContent = content;
            return this;
        }

        public InfoBox<TModel> WithContent(float dataNumber, string content)
        {
            this.dataContentType = ContentType.Number;
            this.dataText = dataNumber.ToString("n0");
            this.dataContent = content;
            return this;
        }

        public InfoBox<TModel> WithContent(int dataNumber, string content)
        {
            this.dataContentType = ContentType.Number;
            this.dataText = dataNumber.ToString("n0");
            this.dataContent = content;
            return this;
        }

        public InfoBox<TModel> WithStatus(StatusType statusType, int statusNumber, bool arrowDown = false)
        {
            this.statusType = statusType;
            this.statusNumber = statusNumber;
            this.statusArrowDown = arrowDown;
            return this;
        }

        public InfoBox<TModel> WithStatus(int percentage)
        {
            bool bellowTarget = percentage < 100;
            this.statusType = bellowTarget ? StatusType.BadgeDownward : StatusType.BadgeUpward;
            this.statusNumber = bellowTarget ? (100 - percentage) : percentage;
            this.statusArrowDown = bellowTarget;
            return this;
        }

        public InfoBox<TModel> DarkStyle(bool enabled)
        {
            this.darkStyle = enabled;
            return this;
        }

        public InfoBox<TModel> ActionUrl(string url)
        {
            this.actionUrl = url;
            return this;
        }
        
        public InfoBox<TModel> Id(string id)
        {
            this.id = id;
            return this;
        }

        public InfoBox<TModel> PullLeft()
        {
            this.cssPull = "pull-left";
            return this;
        }

        public InfoBox<TModel> PullRight()
        {
            this.cssPull = "pull-right";
            return this;
        }

        public override MvcHtmlString Render()
        {
            /*
             *  <div class="infobox infobox-green  ">
                    <div class="infobox-icon">
                        <i class="icon-comments"></i>
                    </div>

                    <div class="infobox-data">
                        <span class="infobox-data-number">32</span>
                        <div class="infobox-content">Firmas diarias</div>
                    </div>
                    <div class="stat stat-success">5%</div>
                </div>
             
             
             * 	<div class="infobox-chart">
				    <span class="sparkline" data-values="373,366,268,298,300,341,815"></span>
                </div>
             * 
               <div class="infobox-progress">
				 <div class="easy-pie-chart percentage" data-percent="12" data-size="46">
				   <span class="percent">12</span>%
				 </div>
			  </div>
              
             */
            var infoBoxTag = new TagBuilder("div");
            if (id != null) 
                infoBoxTag.MergeAttribute("id", this.id);

            infoBoxTag.AddCssClass("infobox");
            infoBoxTag.AddCssClass(infoBoxColor.GetDescription());
            if (darkStyle)
                infoBoxTag.AddCssClass("infobox-small infobox-dark");
            if (cssPull != null)
                infoBoxTag.AddCssClass(cssPull);

            // infobox glyph
            var infoBoxGlyphTag = new TagBuilder("div");
            switch (glyphType)
            {
                case GlypType.Icon:
                    infoBoxGlyphTag.AddCssClass("infobox-icon");
                    string html = String.Format("<i class=\"{0}\"></i>", iconClass);

                    infoBoxGlyphTag.InnerHtml = html;
                    break;
                case GlypType.Progress:
                    var infoBoxProgressGlyph = new TagBuilder("div");
                    infoBoxProgressGlyph.AddCssClass("easy-pie-chart percentage");
                    infoBoxProgressGlyph.MergeAttribute("data-percent", progressPercent.ToString());
                    infoBoxProgressGlyph.MergeAttribute("data-size", darkStyle ? "39" : "46");

                    var infoBoxProgresGlyphPercent = new TagBuilder("div");
                    infoBoxProgresGlyphPercent.AddCssClass("percent");
                    infoBoxProgresGlyphPercent.SetInnerText(progressPercent + "%");
                    infoBoxProgressGlyph.InnerHtml = infoBoxProgresGlyphPercent.ToString();

                    infoBoxGlyphTag.AddCssClass("infobox-progress");
                    infoBoxGlyphTag.InnerHtml = infoBoxProgressGlyph.ToString();
                    break;
                case GlypType.Chart:
                    var infoBoxChartGlyph = new TagBuilder("span");
                    infoBoxChartGlyph.AddCssClass("sparkline");
                    infoBoxChartGlyph.MergeAttribute("data-values", string.Join(",", chartSerieValues));
                    infoBoxChartGlyph.MergeAttribute("data-spark-type", sparklineOptions.Type.ToString().ToLower());
                    infoBoxChartGlyph.MergeAttribute("data-spark-min", sparklineOptions.MinValue.ToString());
                    infoBoxChartGlyph.MergeAttribute("data-spark-max", sparklineOptions.MaxValue.ToString());

                    infoBoxGlyphTag.AddCssClass("infobox-chart");
                    infoBoxGlyphTag.InnerHtml = infoBoxChartGlyph.ToString();
                    break;
            }
            infoBoxTag.InnerHtml = infoBoxGlyphTag.ToString(TagRenderMode.Normal);

            // infobox-data
            /*
                    <div class="infobox-data">
                        <span class="infobox-data-number">32</span>
                        <div class="infobox-content">Firmas diarias</div>
                    </div>
             
                    <div class="infobox-data">
                        <span class="infobox-text">CPU</span>
                        <div class="infobox-content">
                            1.4GB utilizado
                        </div>
                    </div>
            */
            var infoBoxDataTag = new TagBuilder("div");
            infoBoxDataTag.AddCssClass("infobox-data");

            var infoBoxDataInfoTag = new TagBuilder("span");
            infoBoxDataInfoTag.AddCssClass(dataContentType == ContentType.Number ? "infobox-data-number" : "infobox-text");
            infoBoxDataInfoTag.SetInnerText(dataText);
            infoBoxDataTag.InnerHtml += infoBoxDataInfoTag;

            var infoBoxContentTag = new TagBuilder("div");
            infoBoxContentTag.AddCssClass("infobox-content");
            infoBoxContentTag.SetInnerText(dataContent);
            infoBoxDataTag.InnerHtml += infoBoxContentTag;

            infoBoxTag.InnerHtml += infoBoxDataTag.ToString(TagRenderMode.Normal);

            // infobox state
            // <div class="stat stat-success">5%</div>
            // <i class="icon-arrow-down"></i>
            if (statusType != StatusType.None)
            {
                var infoBoxStatTag = new TagBuilder("div");
                infoBoxStatTag.AddCssClass(statusType.GetDescription());
                infoBoxStatTag.InnerHtml = String.Format("%{0}", statusNumber);
                if (statusArrowDown)
                {
                    infoBoxStatTag.InnerHtml += "<i class=\"icon-arrow-down\"></i>";
                }
                infoBoxTag.InnerHtml += infoBoxStatTag.ToString(TagRenderMode.Normal);
            }


            // ahref
            if (actionUrl != null)
            {
                var aTag = new TagBuilder("a");
                aTag.MergeAttribute("href", actionUrl);
                aTag.MergeAttribute("target", "_blank");
                aTag.InnerHtml = infoBoxTag.ToString(TagRenderMode.Normal);
                return MvcHtmlString.Create(aTag.ToString());
            }
            else 
                return MvcHtmlString.Create(infoBoxTag.ToString(TagRenderMode.Normal));
        }

    }
}