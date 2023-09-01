/* ---------------------
 * Add all carousel controls
 * ------------------ */
const carousels = document.querySelectorAll(".carousel");
carousels.forEach(element => {
	element.innerHTML += '<div class="arrow prev"></div><div class="arrow next"></div>';
});


/* ---------------------
 * Carousel controls actions !
 * ------------------ */
function changeSlide(e){
	const arrow = e.target;
	const carousel = arrow.parentElement;
	let cur = parseInt(getComputedStyle(carousel).getPropertyValue("--slide"));
	let inc = 0;
	if(arrow.classList.contains("next")){
		inc += 1;
	}
	if(arrow.classList.contains("prev")){
		inc -= 1;
	}
	if(inc == 0){
		console.error("No direction defined :");
		console.error(arrow.classList);
	}
	const slideNumber = carousel.querySelectorAll(".slide").length;
	let next = (cur + inc + slideNumber) % slideNumber;
	carousel.style.setProperty('--slide', '' + next);
}

document.querySelectorAll(".carousel > .arrow")
	.forEach(element=>{
		element.addEventListener("click", changeSlide, false);
	});

// Turn carousel at regular interval !
setInterval(() => {
	document.querySelectorAll(".carousel > .next")
		.forEach(element=>{
			element.click();
		});
}, 4500);

/* ---------------------
 * Animate aboutUs section (GameMasters) when 
 * they come in view.
 * ------------------ */
const inViewport = (entries, observer) => {
	// Trick from : https://stackoverflow.com/questions/27462306/css3-animate-elements-if-visible-in-viewport-page-scroll
	entries.forEach(entry => {
		entry.target.classList.toggle("is-inViewport", entry.isIntersecting);
	});
};

const Obs = new IntersectionObserver(inViewport);
const obsOptions = {}; //See: https://developer.mozilla.org/en-US/docs/Web/API/Intersection_Observer_API#Intersection_observer_options

// Attach observer to every [data-inviewport] element:
document.querySelectorAll('.popFromRight')
	.forEach(el => {
		Obs.observe(el, obsOptions);
	});