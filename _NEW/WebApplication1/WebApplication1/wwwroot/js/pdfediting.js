$(document).ready(function () {
  /**
   * Click handler for creating a new pdf document
   *
   * This method is simply the entry point for loading a new blank pdf document into the
   * iframe
   *
   **/
  $("#new-pdf-doc-button").click(function (e) {
    /**
     * AJAX call for getting the actual contents of the new pdf file
     *
     * This method send a get request to the OnGetNewPdfFile method in the controller which in
     * turn sends a base64 representation of a blank file back. This string is then converted
     * into a blob, transformed into an object url and said url is set as the src property
     * on the iframe
     *
     * @httptype GET
     **/
    $.ajax({
      url: "?handler=ANewPdfFile",
      type: "get",
      success: (data) => {
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

  /**
   * Click handler for saving the current pdf document to the server
   *
   * This method first gets the file name entered into the input field. Then a fetch request is made
   * to the src property of the iFrame which loads its contents into an array buffer. This array buffer is
   * converted into a base64 string and passed along inside the AJAX call
   *
   **/
  $("#submit-pdf-button").click(async function (e) {
    e.preventDefault();
    let fileName = $("#new-pdf-filename").val();
    let file = await fetch($("#frame").attr("src")).then((res) =>
      res.arrayBuffer()
    );
    let fileAsBase64String = btoa(String.fromCharCode(...new Uint8Array(file)));

    /**
     * AJAX call to send the file to the server
     *
     * Post request is made to the OnPostFile method in the page controller. The data being
     * sent is the base64 representation of the file to be uploaded.
     *
     * @httptype POST
     *
     **/
    $.ajax({
      url: "?handler=File&fileName=" + fileName + "&typeOfFile=pdf",
      type: "post",
      contentType: false,
      processData: false,
      data: fileAsBase64String,
      success: function () {
        alert(fileName + ".pdf was successfully saved.");
      },
      error: function (err) {
        alert("Error posting the file." + "\n" + err);
      },
    });
  });

  /**
   * Retrieve the desired file from our database to be loaded into the front end
   *
   * When a file inside open a pdf modal is clicked this method is fired. The ID of the file is obtained
   * from the modal and then passed as a parameter inside the AJAX call to get it.
   *
   **/
  $(".file-in-db").on("click", function (e) {
    e.preventDefault();
    //Find the ID of the file to get from the modal
    let id = $(this).parent().siblings(".sister").find(".file-id").text();

    /**
     * AJAX call to get the file to be displayed.
     *
     * The AJAX call recieves a base64
     * representation of the file when it is successful. Then a blob object is created from the base64 string. From this blob
     * an Object Url is created on the browser to store the file. Finally the URL of the file is set to the iFrame's source which
     * in turn displays it
     *
     *@httptype GET
     **/
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
      },
      error: function () {
        alert("Could not find a file.");
      },
    });
    $("#open-pdf-modal").toggle();
  });
  $("#split-pdf-submit-button").on("click", function (e){
    e.preventDefault();
    $("#split-pdf-modal").hide();
  });
});
