import Youtube from 'react-youtube';

const option = {
    playerVars: {
        autoplay: 0,
        mute: 1,
        height: 720,
        width:1080
    }
}

export default function WebCamComponent() {
    return(
    <div class="block m-3 p-4 bg-white border border-gray-200 rounded-lg shadow-md dark:bg-gray-800 dark:border-gray-700">
        <h5 class="mb-2 text-2xl font-bold tracking-tight text-gray-900 dark:text-white">Webカメラ</h5>
        {/*<Youtube className='m-1' videoId='3kPH7kTphnE' opts={option}/>*/}
        <p className='text-center'>映像</p>
    </div>
    )
}