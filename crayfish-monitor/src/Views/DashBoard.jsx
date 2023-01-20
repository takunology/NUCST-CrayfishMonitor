import React from 'react';
import ChartComponent from '../components/ChartComponent';
import MeasurementTableComponent from '../components/MeasurementTableComponent';

function DashBoard () {
    return (
    <div className='row'>
        <div className='col'>
            <div class="block bg-white py-3 m-3 border border-gray-200 rounded-lg shadow-md hover:bg-gray-100 dark:bg-gray-800 dark:border-gray-700 dark:hover:bg-gray-800">
                <h5 class="mx-2 text-2xl tracking-tight text-gray-900 dark:text-white">個体Aのデータ例</h5>
                <ChartComponent/>
            </div>
        </div>
        <div className='col'>
            <div className='relative overflow-x-auto shadow-md sm:rounded-lg m-3'>
                <MeasurementTableComponent/>
            </div>
        </div>
    </div>
    );
}

export default DashBoard;