window.onload = () => {
    $("body").on("input", () => {
        if (document.activeElement.tagName === "INPUT") {
            var input = document.activeElement;

            if (input.hasAttribute("list")) {
                var children = $("#" + input.getAttribute('list')).children();
                var containsValidInput = false;

                for (i = 0; children.length > i; i++) {
                    if (input.value.trim() === children[i].value.trim() || input.value === "") {
                        containsValidInput = true;
                    }
                }

                if (containsValidInput) {
                    input.style = "box-shadow: 0 0 0 0.2rem rgba(0, 255, 0, 0.25);"
                }
                else {
                    input.style = "box-shadow: 0 0 0 0.2rem rgba(255, 0, 0, 0.25);"
                }

            }
        }
    })
}
    