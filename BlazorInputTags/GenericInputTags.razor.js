export function initialize(id, dotNetHelper) {
    var element = document.getElementById(id);

    element.addEventListener('keydown', function (e) {
        if (e.key === "Enter" || e.key === "ArrowDown" || e.key === "ArrowUp" || e.key === "Escape") {
            e.stopPropagation();
            e.preventDefault();
        }
    });

    element.addEventListener('keyup', function (e) {
        if (e.key === "Enter" || e.key === "ArrowDown" || e.key === "ArrowUp" || e.key === "Escape") {
            e.stopPropagation();
            e.preventDefault();
        }

        if (e.key === "Enter") {
            dotNetHelper.invokeMethodAsync("OnItemSelectedAsync");
        } else if (e.key === "ArrowDown") {
            dotNetHelper.invokeMethodAsync("SelectNextItemAsync");
        } else if (e.key === "ArrowUp") {
            dotNetHelper.invokeMethodAsync("SelectPreviousItemAsync");
        } else if (e.key === "Escape") {
            dotNetHelper.invokeMethodAsync("HideSearchResultsAsync");
        }

    });
}