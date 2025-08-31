function preview(inputType) {
    var file = inputType.files[0];
    var allowTypes = "image.*";
    if (file.type.match(allowTypes)) {
        $(".btn").prop("disabled", false);
        var reader = new FileReader();
        reader.onload = function (e) {
            $("#Picture").prev().attr("src", e.target.result);
            $("#Picture").prev().attr("title", file.name);
        }
        reader.readAsDataURL(file);
    }
    else {
        alert("不允許的檔案上傳類型");
        $(".btn").prop("disabled", true);
        inputType.value = "";
        $("#Picture").prev().attr("src", "/images/no_image.jpg");
        $("#Picture").prev().attr("title", "尚無圖片");

    }
}
$("#Picture").on("change", function () {
    // alert("change");(有選到不同檔案，會出現change)

    preview(this);
})