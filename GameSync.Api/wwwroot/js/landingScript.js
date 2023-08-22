new fullpage('#fullpage', {
    verticalCentered: false,
    licenseKey: "gplv3-license"
});

setInterval(() => {document.querySelector(".fp-arrow.fp-controlArrow.fp-next").click();}, 3500);