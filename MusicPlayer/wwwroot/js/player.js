
document.getElementById("playpause").addEventListener("click", playpause);
document.getElementById("next").addEventListener("click", next);
document.getElementById("previous").addEventListener("click", previous);
document.getElementById("muteunmute").addEventListener("click", muteunmute);
const filepath = document.getElementById("filepath").textContent


console.log("filepath" + filepath)


const wavesurfer = WaveSurfer.create({
    container: '#waveform',
    scrollParent: true
});
if (  !(filepath === "")) {
    wavesurfer.load(  window.location.origin  + '/songs/' + filepath);
}else{
    document.getElementById("playpause").disabled = true;
    document.getElementById("next").disabled = true;
    document.getElementById("previous").disabled = true;
    document.getElementById("muteunmute").disabled = true;
    document.getElementById("removesong").disabled = true;

    
    console.log("filepath is empty")
}
wavesurfer.on('ready', function () {
   console.log("song is ready")
    document.getElementById("songtime").innerText = "Time : ~ " +Math.round(wavesurfer.getDuration() /60.0)  + " min"
    window.setInterval(getcurrentime, 1000)
    wavesurfer.playPause();
});

function getcurrentime(e){
    document.getElementById("currentime").innerText = "Current time : " + Math.round(  wavesurfer.getCurrentTime()) + " s"
}

function muteunmute(e){
        wavesurfer.toggleMute() ;
}

function playpause(e) {
    if (!wavesurfer.isReady){
        alert("please wait, still loading")
        return;
    }
    wavesurfer.playPause();
}

function next(e) {
    
}
function  previous(e){

}

