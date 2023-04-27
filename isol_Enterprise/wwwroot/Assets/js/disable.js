 
	/*!
devtools-detect
https://github.com/sindresorhus/devtools-detect
By Sindre Sorhus
MIT License
*/

const devtools = {
	isOpen: false,
	orientation: undefined,
};

const threshold = 170;

const emitEvent = (isOpen, orientation) => {
	globalThis.dispatchEvent(new globalThis.CustomEvent('devtoolschange', {
		detail: {
			isOpen,
			orientation,
		},
	}));
};

const main = ({emitEvents = true} = {}) => {
	const widthThreshold = globalThis.outerWidth - globalThis.innerWidth > threshold;
	const heightThreshold = globalThis.outerHeight - globalThis.innerHeight > threshold;
	const orientation = widthThreshold ? 'vertical' : 'horizontal';

	if (
		!(heightThreshold && widthThreshold)
		&& ((globalThis.Firebug && globalThis.Firebug.chrome && globalThis.Firebug.chrome.isInitialized) || widthThreshold || heightThreshold)
	) {
		if ((!devtools.isOpen || devtools.orientation !== orientation) && emitEvents) {
			emitEvent(true, orientation);
		}

		devtools.isOpen = true;
		devtools.orientation = orientation;
	} else {
		if (devtools.isOpen && emitEvents) {
			emitEvent(false, undefined);
		}

		devtools.isOpen = false;
		devtools.orientation = undefined;
	}
};

main({emitEvents: false});
setInterval(main, 500);

if(devtools.isOpen){

		toastr.warning("Refresh your page !!!");
			
setTimeout(myfun, 5000);


 

 			
		}
// Check if it's open
	console.log('Is DevTools open:', devtools.isOpen);
 
		//if(event.detail.isOpen){
		//var a = document.getElementsByTagName('HTML')[0];	
		//toastr.warning("Refresh your page !!!");
		//setTimeout(myfun, 5000); }



	window.addEventListener('devtoolschange', event => {
		//console.log('Is DevTools open:', event.detail.isOpen);
		//alert(event.detail.isOpen)
		if(event.detail.isOpen){

		toastr.warning("Refresh your page !!!");
			
setTimeout(myfun, 5000);


 

 			
		}
		 
	});



	// Disable all inspect buttons
	document.onkeydown = function(e) {
		if (e.keyCode == 123) {
    return false;
 }
 if(e.ctrlKey && e.shiftKey && e.keyCode == 'I'.charCodeAt(0)) {
    return false;
 }
 if(e.ctrlKey && e.shiftKey && e.keyCode == 'C'.charCodeAt(0)) {
    return false;
 }
 if(e.ctrlKey && e.shiftKey && e.keyCode == 'J'.charCodeAt(0)) {
    return false;
 }
 if(e.ctrlKey && e.keyCode == 'U'.charCodeAt(0)) {
    return false;
 }
} 
 


function myfun(){
		var a = document.getElementsByTagName('HTML')[0];	
		a.innerHTML = "<h1>Refresh your page !!!</h1>";
}