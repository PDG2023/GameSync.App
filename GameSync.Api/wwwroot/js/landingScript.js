new fullpage('#fullpage', {
    verticalCentered: false,
    licenseKey: "gplv3-license"
});

setInterval(() => {document.querySelector(".fp-arrow.fp-controlArrow.fp-next").click();}, 3500);

const inViewport = (entries, observer) => {
    // Trick from : https://stackoverflow.com/questions/27462306/css3-animate-elements-if-visible-in-viewport-page-scroll
    entries.forEach(entry => {
        entry.target.classList.toggle("is-inViewport", entry.isIntersecting);
    });
};

const Obs = new IntersectionObserver(inViewport);
const obsOptions = {}; //See: https://developer.mozilla.org/en-US/docs/Web/API/Intersection_Observer_API#Intersection_observer_options

// Attach observer to every [data-inviewport] element:
document.querySelectorAll('.popFromRight').forEach(el => {
    Obs.observe(el, obsOptions);
});