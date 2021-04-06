$(document).ready(function () {
  $("#new-pdf-doc-button").click(function (e) {
    e.preventDefault();
    if (
      confirm(
        "Are you sure you want to make a new pdf file? Unsaved changes will be lost."
      )
    ) {
      // createANewPdf();
    }
  });

  $("#submit-pdf-button").click(function (e) {
    e.preventDefault();
    $.ajax({
      url: "",
      data: new FormData(document.forms[0]),
      contentType: false,
      processData: false,
      type: "post",
      success: function () {
        alert("Uploaded by jQuery");
      },
    });
  });

  $(".file-in-db").on("click", function (e) {
    e.preventDefault();
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
  });
});
