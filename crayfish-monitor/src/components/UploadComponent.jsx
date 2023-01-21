import { ReadFile, mapCSVToArray } from '../API/FileReader'
import ChartComponent from './ChartComponent';

export default function UploadComponent() {
    return (
        <div class="flex items-center justify-center m-3">
        <label for="dropzone-file" class="flex flex-col items-center justify-center w-full h-64 border-2 border-gray-300 border-dashed rounded-lg cursor-pointer bg-white dark:hover:bg-bray-800 dark:bg-gray-800 hover:bg-gray-50 dark:border-gray-600 dark:hover:border-gray-500 dark:hover:bg-gray-700">
            <div class="flex flex-col items-center justify-center pt-5 pb-6">
                <svg aria-hidden="true" class="w-10 h-10 mb-3 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 16a4 4 0 01-.88-7.903A5 5 0 1115.9 6L16 6a5 5 0 011 9.9M15 13l-3-3m0 0l-3 3m3-3v12"></path></svg>
                <p class="mb-2 text-md text-gray-500 dark:text-gray-400"><span class="font-semibold">計測データのアップロード</span></p>
                <p class="text-sm text-gray-500 dark:text-gray-400">txt または csv ファイル</p>
            </div>
            <input id="dropzone-file" type="file" class="hidden" onChange={FileChanged(this)}/>
            
        </label>
    </div> 
    )
}

function FileChanged(file){
    try{
        const csv = ReadFile(file);
        const arr = mapCSVToArray(csv);
        return (
            <ChartComponent data_x={arr[1]} data_y={arr[2]}/>
        )
    } catch (error) {
        console.log(error);
    }
}