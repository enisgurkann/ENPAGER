using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ENPAGER;

[HtmlTargetElement("EnPager", TagStructure = TagStructure.WithoutEndTag)]
public class PagedTagHelper : TagHelper
{
    #region Model Values

    [HtmlAttributeName("page")] public int Page { get; set; } = 1;

    [HtmlAttributeName("page-size")] public int PageSize { get; set; } = 20;

    [HtmlAttributeName("total-pages")] public int TotalPages { get; set; }

    [HtmlAttributeName("total-items")] public int Total { get; set; }

    [HtmlAttributeName("parameter-name")] public string QueryStringName { get; set; } = "page";

    #endregion

    #region Class Names

    [HtmlAttributeName("first-class")] public string FirstButtonClass { get; set; } = "first-page";

    [HtmlAttributeName("last-class")] public string LastButtonClass { get; set; } = "last-page";

    [HtmlAttributeName("prev-class")] public string PrevButtonClass { get; set; } = "prev-page";

    [HtmlAttributeName("next-class")] public string NextButtonClass { get; set; } = "next-page";

    [HtmlAttributeName("main-class")] public string MainClass { get; set; } = "pagination";

    [HtmlAttributeName("item-class")] public string ItemClass { get; set; } = "page-item";

    [HtmlAttributeName("link-class")] public string ItemLinkClass { get; set; } = "page-link";

    [HtmlAttributeName("active-class")] public string ActiveItemClass { get; set; } = "active";

    [HtmlAttributeName("active-link-class")]
    public string ActiveLinkClass { get; set; } = "active";

    #endregion

    #region Options

    [HtmlAttributeName("show-first")] public bool ShowFirst { get; set; } = true;

    [HtmlAttributeName("first-text")] public string FirstButtonText { get; set; } = "İlk";

    [HtmlAttributeName("show-last")] public bool ShowLast { get; set; } = true;

    [HtmlAttributeName("last-text")] public string LastButtonText { get; set; } = "Son";

    [HtmlAttributeName("show-prev")] public bool ShowPrev { get; set; } = true;

    [HtmlAttributeName("prev-text")] public string PrevButtonText { get; set; } = "Önceki";

    [HtmlAttributeName("show-next")] public bool ShowNext { get; set; } = true;

    [HtmlAttributeName("next-text")] public string NextButtonText { get; set; } = "Sonraki";

    [HtmlAttributeName("show-active-link")]
    public bool ShowActiveLink { get; set; } = false;

    [HtmlAttributeName("show-individual")] public bool ShowIndividualPages { get; set; } = true;

    #endregion

    [ViewContext] [HtmlAttributeNotBound] public ViewContext ViewContext { get; set; }

    private int PageIndex => Page - 1;
    private int IndividualPagesDisplayedCount = 5;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (Total == 0)
        {
            output.SuppressOutput();
            return;
        }

        output.Attributes.Add("class", MainClass);
        if (ShowFirst)
        {
            //first page
            if (PageIndex >= 3)
            {
                var firstButton = CreatePageLink(1, FirstButtonText);

                if (!string.IsNullOrEmpty(FirstButtonClass))
                    firstButton.AddCssClass(FirstButtonClass);

                output.Content.AppendHtml(firstButton);
            }
        }

        if (ShowPrev)
        {
            //previous page
            if (PageIndex > 0)
            {
                var prevButton = CreatePageLink(PageIndex, PrevButtonText);

                if (!string.IsNullOrEmpty(PrevButtonClass))
                    prevButton.AddCssClass(PrevButtonClass);

                output.Content.AppendHtml(prevButton);
            }
        }

        if (ShowIndividualPages)
        {
            //individual pages
            var firstIndividualPageIndex = GetFirstIndividualPageIndex();
            var lastIndividualPageIndex = GetLastIndividualPageIndex();
            for (var i = firstIndividualPageIndex; i <= lastIndividualPageIndex; i++)
            {
                output.Content.AppendHtml(CreatePageLink(i + 1, (i + 1).ToString(), currentPage: PageIndex == i));
            }
        }

        if (ShowNext)
        {
            //next page
            if ((PageIndex + 1) < TotalPages)
            {
                var nextButton = CreatePageLink(PageIndex + 2, NextButtonText);

                if (!string.IsNullOrEmpty(NextButtonClass))
                    nextButton.AddCssClass(NextButtonClass);

                output.Content.AppendHtml(nextButton);
            }
        }

        if (ShowLast)
        {
            //last page
            if ((PageIndex + 3) < TotalPages && TotalPages > IndividualPagesDisplayedCount)
            {
                var lastButton = CreatePageLink(TotalPages, LastButtonText);

                if (!string.IsNullOrEmpty(LastButtonClass))
                    lastButton.AddCssClass(LastButtonClass);

                output.Content.AppendHtml(lastButton);
            }
        }

        //tag details
        output.TagName = "ul";
        output.TagMode = TagMode.StartTagAndEndTag;
    }

    private int GetFirstIndividualPageIndex()
    {
        if (TotalPages < IndividualPagesDisplayedCount || (PageIndex - (IndividualPagesDisplayedCount / 2) < 0))
        {
            return 0;
        }

        if (PageIndex + (IndividualPagesDisplayedCount / 2) >= TotalPages)
        {
            return (TotalPages - IndividualPagesDisplayedCount);
        }

        return (PageIndex - (IndividualPagesDisplayedCount / 2));
    }

    private int GetLastIndividualPageIndex()
    {
        var num = IndividualPagesDisplayedCount / 2;
        if ((IndividualPagesDisplayedCount % 2) == 0)
        {
            num--;
        }

        if (TotalPages < IndividualPagesDisplayedCount || (PageIndex + num) >= TotalPages)
        {
            return (TotalPages - 1);
        }

        if ((PageIndex - (IndividualPagesDisplayedCount / 2)) < 0)
        {
            return (IndividualPagesDisplayedCount - 1);
        }

        return (PageIndex + num);
    }

    private TagBuilder CreatePageLink(int pageNumber, string text, string className = null, bool currentPage = false)
    {
        var liBuilder = new TagBuilder("li");
        liBuilder.AddCssClass(ItemClass);

        if (!string.IsNullOrEmpty(className))
            liBuilder.AddCssClass(className);

        if (!currentPage || ShowActiveLink)
        {
            var linkBuilder = new TagBuilder("a");
            linkBuilder.InnerHtml.AppendHtml(text);
            linkBuilder.AddCssClass(ItemLinkClass);

            if (currentPage)
            {
                liBuilder.AddCssClass(ActiveItemClass);

                if (!string.IsNullOrEmpty(ActiveLinkClass))
                    linkBuilder.AddCssClass(ActiveLinkClass);
            }
            else
                linkBuilder.MergeAttribute("href", string.Format(CreateUrlTemplate(), pageNumber));

            liBuilder.InnerHtml.AppendHtml(linkBuilder);
        }
        else
        {
            liBuilder.InnerHtml.AppendHtml(text);
        }

        return liBuilder;
    }

    private string CreateUrlTemplate()
    {
        var urlPath = ViewContext.HttpContext.Request.QueryString.Value;

        var urlTemplate = string.IsNullOrWhiteSpace(urlPath) ? $"{QueryStringName}=1".Split('&').ToList() : urlPath.TrimStart('?').Split('&').ToList();

        var queryStrings = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        urlTemplate.ForEach(x => queryStrings.Add(x.Split('=')[0], x.Split('=')[1]));

        if (!queryStrings.ContainsKey(QueryStringName))
            queryStrings.Add(QueryStringName, "{0}");
        else
            queryStrings[QueryStringName] = "{0}";

        return "?" + string.Join("&", queryStrings.Select(q => q.Key + "=" + q.Value));
    }
}