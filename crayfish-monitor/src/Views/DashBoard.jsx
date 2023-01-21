import React from 'react';
import ChartComponent from '../components/ChartComponent';
import HartRateComponent from '../components/HartRateComponent';
import MeasurementTableComponent from '../components/MeasurementTableComponent';
import WebCamComponent from '../components/WebCamComponent';
import MeasurementSelectComponent from '../components/MeasurementSelectComponent'

function DashBoard () {
    return (
    <div className='flex flex-col'>
        <div class="block bg-white py-3 mx-3 mt-3 border border-gray-200 rounded-lg shadow-md dark:bg-gray-800 dark:border-gray-700 dark:hover:bg-gray-800">
            <h5 class="mx-3 text-2xl font-bold tracking-tight text-gray-900 dark:text-white">個体A</h5>
        </div>
        <div class="block bg-white py-3 pr-5 m-3 border border-gray-200 rounded-lg shadow-md dark:bg-gray-800 dark:border-gray-700 dark:hover:bg-gray-800">
            <h5 class="mx-3 text-2xl font-bold tracking-tight text-gray-900 dark:text-white">過去30分の計測データ</h5>
            <ChartComponent/>
        </div>
        <div className='relative overflow-x-auto shadow-md m-3 sm:rounded-lg'>
            <MeasurementTableComponent/>
        </div>
        <div className='grid lg:grid-cols-2 xl:grid-cols-3'>
            <WebCamComponent />
            <MeasurementSelectComponent />
            <HartRateComponent />
        </div>
    </div>
    );
}

export default DashBoard;