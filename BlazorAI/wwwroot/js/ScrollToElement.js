function scrollToElement(elementId) {
    var elem = document.getElementById(elementId);

    if (elem.offsetTop < self.pageYOffset) {
        elem.scrollIntoView({ behavior: 'smooth' });
    }
};