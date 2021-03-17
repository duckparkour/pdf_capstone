

$(document).ready(function () {

    $('#new-pdf-doc-button').click(function (e) {
        e.preventDefault();
        if (confirm('Are you sure you want to make a new pdf file? Unsaved changes will be lost.')) {
            createANewPdf();
        }
    });

    /*$('#add-text-button').click(function (e) {
        e.preventDefault();
        addTextToPdf();
    });*/

    /*async function createPdf() {
        const pdfDoc = await PDFLib.PDFDocument.create();
        const page = pdfDoc.addPage([350, 400]);
        page.moveTo(110, 200);
        page.drawText('Hello Worrld!');
        const pdfDataUri = await pdfDoc.saveAsBase64({ dataUri: true });
        document.getElementById('pdf').src = pdfDataUri;

    }*/

    async function createANewPdf() {
        const pdfDoc = await PDFLib.PDFDocument.create();
        const page = pdfDoc.addPage([350, 400]);
        const pdfDataUri = await pdfDoc.saveAsBase64({ dataUri: true });
        document.getElementById('frame').src = pdfDataUri;

    }

    /*async function addTextToPdf() {
        const currentDocument = document.getElementById('pdf').src
        const existingDocumentsBytes = await fetch(currentDocument)
            .then(response => response.arrayBuffer())
        const pdfDoc = await PDFLib.PDFDocument.load(existingDocumentsBytes)
        const helveticaFont = await pdfDoc.embedFont(PDFLib.StandardFonts.Helvetica)

        const pages = pdfDoc.getPages()
        const firstPage = pages[0]
        const { width, height } = firstPage.getSize()
        firstPage.drawText('This text was added with JavaScript!', {
            x: 5,
            y: height / 2 + 300,
            size: 50,
            font: helveticaFont,
            color: PDFLib.rgb(0.95, 0.1, 0.1),
            rotate: PDFLib.degrees(-45),
        })

        const pdfDataUri = await pdfDoc.saveAsBase64({ dataUri: true });
        document.getElementById('pdf').src = pdfDataUri;
    }*/
});