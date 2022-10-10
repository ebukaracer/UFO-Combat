using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Handles page/panel transitions(fade-in/fade-out)
/// </summary>
internal class PageSwitchAnimation : MonoBehaviour
{
    private CanvasGroup _previousPage;
    private CanvasGroup _currentPage;

    private int _currentPageNumber;

    [SerializeField] private List<CanvasGroup> pages;

    [Space(5), SerializeField] private float fadeDuration = .25f;

    private void Awake()
    {
        // Excluding the first page, fade-out/disable other pages.
        for (int i = 1; i < pages.Count; i++)
        {
            var page = pages[i];

            page.alpha = 0;

            page.blocksRaycasts = false;
        }

        // Current page set to the first page-item on the [pages] list.
        _currentPage = pages[0];

        _currentPageNumber = 0;
    }

    /// <summary>
    /// Each page has its unique number assigned to the inspector.
    /// This is assigned to buttons(their callbacks) that'd fade in/out a new page when clicked.
    /// </summary>
    /// <param name="pageNumber">Current page unique number.</param>
    public void Page(int pageNumber)
    {
        SwitchPage(pageNumber);
    }

    /// <summary>
    /// Sets a page active/inactive based on its number.
    /// </summary>
    /// <param name="pageNumber">The current page's number to enable/disable</param>
    private void SwitchPage(int pageNumber)
    {
        if (_currentPageNumber == pageNumber)
            return;

        // Sets the previous page inactive
        _previousPage = _currentPage;

        _previousPage.blocksRaycasts = false;

        _previousPage.DOFade(0, fadeDuration).OnComplete
        (
            delegate
            {
                // Sets a new page active
                _currentPage = pages[pageNumber];

                _previousPage = _currentPage.GetComponent<CanvasGroup>();

                _previousPage.DOFade(1, fadeDuration);

                _previousPage.blocksRaycasts = true;

                _currentPageNumber = pageNumber;
            }
        );
    }
}
