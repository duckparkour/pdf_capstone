$(document).ready(function () {
  $("#new-pdf-doc-button").click(function (e) {
    // e.preventDefault();
    // if (
    //   confirm(
    //     "Are you sure you want to make a new pdf file? Unsaved changes will be lost."
    //   )
    // ) {
    //   createANewPdf();
    // }
    $.ajax({
      url: "?handler=ANewPdfFile&fileName=testfile",
      type: "get",
      success: (data) => {
        const url = "data:application/pdf;base64," + data;
        fetch(url)
          .then((res) => res.blob())
          .then((res) => {
            const url = URL.createObjectURL(res);
            $("#frame").attr("src", url);
          });

        $("#frame").attr("src", "data:application/pdf;base64," + data);
      },

      error: (data) => {
        alert("operation failed");
      },
    });
  });
    $("#submit-pdf-button").click(function (e) {
    e.preventDefault();
    let form = document.getElementById('upload-form');
    $.ajax({
      url: "",
      data: new FormData(form),
      contentType: false,
      processData: false,
      type: "post",
      success: function () {
        alert("success");
      },
    });
  });
  $(".file-in-db").on("click", function (e) {
    e.preventDefault();
    //Find the ID of the file to get from the modal
    let id = $(this).parent().siblings(".sister").find(".file-id").text();

    $.ajax({
      url: "?handler=File&ID=" + id,
      type: "get",
      success: function (data) {
        const url = "data:application/pdf;base64," + data;
        fetch(url)
          .then((res) => res.blob())
          .then((res) => {
            const url = URL.createObjectURL(res);
            $("#frame").attr("src", url);
          });

        $("#frame").attr("src", "data:application/pdf;base64," + data);
      },
      error: function () {
        alert("Could not find a file.");
      },
    });
    $("#open-pdf-modal").toggle();
  });
});
