export function initialize(id, dotNetHelper) {
    var element = document.getElementById(id);

    element.addEventListener('keyup', function (e) {

        if (e.key === "Enter") {
            e.stopPropagation();
            e.preventDefault();
            dotNetHelper.invokeMethodAsync("OnItemSelectedAsync");
        } else if (e.key == "ArrowDown") {
            e.stopPropagation();
            e.preventDefault();
            dotNetHelper.invokeMethodAsync("SelectNextItemAsync");
        } else if (e.key == "ArrowUp") {
            e.stopPropagation();
            e.preventDefault();
            dotNetHelper.invokeMethodAsync("SelectPreviousItemAsync");
        }

    });
}