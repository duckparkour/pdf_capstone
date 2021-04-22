$(document).ready(function () {
  let currentPdfAsBase64;

  $("#new-pdf-doc-button").click(function (e) {
    $.ajax({
      url: "?handler=ANewPdfFile&fileName=testfile",
      type: "get",
      success: (data) => {
        currentPdfAsBase64 = data;
        const url = "data:application/pdf;base64," + data;
        fetch(url)
          .then((res) => res.blob())
          .then((res) => {
            const url = URL.createObjectURL(res);
            $("#frame").attr("src", url);
          });
      },

      error: (data) => {
        alert("operation failed");
      },
    });
  });

  $("#submit-pdf-button").click(async function (e) {
    e.preventDefault();
    let fileName = $("#new-pdf-filename").val();
    let file = await fetch($("#frame").attr("src")).then((res) =>
      res.arrayBuffer()
    );
    let fileAsBase64String = btoa(String.fromCharCode(...new Uint8Array(file)));

    $.ajax({
      url: "?handler=NewPdf&fileName=" + fileName,
      type: "post",
      contentType: false,
      processData: false,
      data: fileAsBase64String,
      success: function () {
        alert("file saved");
        console.log(fileName);
      },
      error: function (err) {
        alert("error");
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
        currentPdfAsBase64 = data;
        const url = "data:application/pdf;base64," + data;
        fetch(url)
          .then((res) => res.blob())
          .then((res) => {
            const url = URL.createObjectURL(res);
            $("#frame").attr("src", url);
          });
      },
      error: function () {
        alert("Could not find a file.");
      },
    });
    $("#open-pdf-modal").toggle();
  });
});
