
const wavesurfer = WaveSurfer.create({
    container: '#waveform',
    scrollParent: true
});
wavesurfer.load('../test.mp3');
wavesurfer.on('ready', function () {
    //   wavesurfer.play();
});

document.getElementById("playpause").addEventListener("click", playpause);
document.getElementById("next").addEventListener("click", next);
document.getElementById("previous").addEventListener("click", previous);
document.getElementById("muteunmute").addEventListener("click", muteunmute);



function muteunmute(e){
        wavesurfer.toggleMute() ;
}

function playpause(e) {
    if (!wavesurfer.isReady){

        alert("please wait, still loading")
        return;
    }
    wavesurfer.playPause() ();
}

function next(e) {
    
}
function  previous(e){

}

