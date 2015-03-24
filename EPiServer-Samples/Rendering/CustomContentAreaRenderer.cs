using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EPiServerSamples.Rendering
{
    public class CustomContentAreaRenderer : ContentAreaRenderer
    {
        private string _FirstClass;
        private string _LastClass;
        private bool _SuppressWrapper;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomContentAreaRenderer"/> class.
        /// </summary>
        /// <param name="contentRenderer"></param>
        /// <param name="templateResolver"></param>
        /// <param name="attributeAssembler"></param>
        /// <param name="contentRepository"></param>
        /// <param name="displayOptions"></param>
        public CustomContentAreaRenderer(IContentRenderer contentRenderer, TemplateResolver templateResolver, ContentFragmentAttributeAssembler attributeAssembler, IContentRepository contentRepository, DisplayOptions displayOptions)
            : base(contentRenderer, templateResolver, attributeAssembler, contentRepository, displayOptions)
        {

        }

        /// <summary>
        /// Override Render to add in custom additionalViewData 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="contentArea"></param>
        public override void Render(HtmlHelper htmlHelper, ContentArea contentArea)
        {
            var cssClass = htmlHelper.ViewData["FirstClass"];
            _FirstClass = cssClass == null ? "first" : cssClass.ToString();

            cssClass = htmlHelper.ViewData["LastClass"];
            _LastClass = cssClass == null ? "last" : cssClass.ToString();

            var toggle = htmlHelper.ViewData["SuppressWrapper"];

            if (toggle is bool)
                _SuppressWrapper = (bool)toggle;

            base.Render(htmlHelper, contentArea);
        }
        
        /// <summary>
        /// Allows for first and last css classes
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="contentAreaItems"></param>
        protected override void RenderContentAreaItems(HtmlHelper htmlHelper, IEnumerable<ContentAreaItem> contentAreaItems)
        {
            int index = 0;
            int total = contentAreaItems.Count();

            foreach (ContentAreaItem contentAreaItem in contentAreaItems)
            {
                this.RenderContentAreaItem
                (
                    htmlHelper,
                    contentAreaItem,
                    this.GetContentAreaItemTemplateTag(htmlHelper, contentAreaItem),
                    this.GetContentAreaItemHtmlTag(htmlHelper, contentAreaItem),
                    CustomGetContentAreaItemCssClass(htmlHelper, contentAreaItem, index, total)
                );

                index++;
            }
        }

        /// <summary>
        /// Allows for the wrapping tag to be disabled
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        protected override bool ShouldRenderWrappingElement(HtmlHelper htmlHelper)
        {
            if (_SuppressWrapper)
            {
                return !_SuppressWrapper;
            }

            return base.ShouldRenderWrappingElement(htmlHelper);
        }

        /// <summary>
        /// Allows for additional attributes like style, data- to be added to containing div wrapping the content area item
        /// </summary>
        /// <param name="tagBuilder"></param>
        /// <param name="contentAreaItem"></param>
        protected override void BeforeRenderContentAreaItemStartTag(TagBuilder tagBuilder, ContentAreaItem contentAreaItem)
        {
            var content = contentAreaItem.GetContent(ContentRepository);
            var customAttrs = content as ICustomAttributesInContentArea;

            if (customAttrs != null)
            {
                tagBuilder.MergeAttributes(customAttrs.CustomAttributes, customAttrs.ReplaceExisitingAttributes);
            }

            base.BeforeRenderContentAreaItemStartTag(tagBuilder, contentAreaItem);
        }

        /// <summary>
        /// Creates custom CSS string based on tag and allows for further Customization for PageData and BlockData that implement ICustomCssInContentArea
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="contentAreaItem"></param>
        /// <returns></returns>
        protected virtual string CustomGetContentAreaItemCssClass(HtmlHelper htmlHelper, ContentAreaItem contentAreaItem, int index, int total)
        {
            var tag = GetContentAreaItemTemplateTag(htmlHelper, contentAreaItem);
            var content = contentAreaItem.GetContent(ContentRepository);
            var customClassContent = content as ICustomCssInContentArea;
            string customCss = string.Empty;

            // Added custom class if one exists
            if (customClassContent != null && !string.IsNullOrWhiteSpace(customClassContent.ContentAreaCssClass))
            {
                customCss = customClassContent.ContentAreaCssClass;
            }

            string firstLastClass = string.Empty;

            if (index == 0)
            {
                firstLastClass = _FirstClass;
            }
            else if (index == total + 1)
            {
                firstLastClass = _LastClass;
            }

            return string.Format("content-block {0} {1} {2} {3}", customCss, GetCssClassForTag(tag), tag, firstLastClass).Trim();
        }

        /// <summary>
        // Gets a CSS class used for styling based on a tag name (ie a Bootstrap class name)
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        private static string GetCssClassForTag(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                return string.Empty;
            }

            switch (tagName.ToLower())
            {
                case "span12":
                    return "col-md-12";
                case "span8":
                    return "col-md-8";
                case "span6":
                    return "col-md-6";
                case "span4":
                    return "col-md-4";
                default:
                    return string.Empty;
            }
        }        
    }
}