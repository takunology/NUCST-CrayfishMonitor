import { MeasurementList } from "./MeasurementList"

export default function MeasurementTableComponent(){
    return (
    <div class="block bg-white p-3 border border-gray-200 rounded-lg shadow-md dark:bg-gray-800 dark:border-gray-700 dark:hover:bg-gray-800">
        <h5 class="mx-2 text-2xl font-bold tracking-tight text-gray-900 dark:text-white">5分おきの計測データ</h5>   
        <table class="w-full mt-3 text-sm text-left overflow-x-auto text-gray-500 dark:text-gray-700">
            <thead class="text-xs bg-gray-100 text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-200">
                <tr>
                    <th scope="col" class="px-6 py-3">
                        ラベル名
                    </th>
                    <th scope="col" class="px-6 py-3">
                        計測日
                    </th>
                    <th scope="col" class="px-6 py-3">
                        計測時間
                    </th>
                    <th scope="col" class="px-6 py-3">
                        
                    </th>
                </tr>
            </thead>
            <tbody>
            {MeasurementList.map((item, key) => {
                return (
                <tr className="bg-white border-b hover:bg-gray-100 dark:bg-gray-800 dark:border-gray-500 dark:hover:bg-gray-700">
                    <th scope="row" class="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white">
                    {item.label}
                    </th>
                    <td class="px-6 py-4 dark:text-gray-200">
                    {item.date}
                    </td>
                    <td class="px-6 py-4 dark:text-gray-200">
                    {item.time}
                    </td>
                    <td class="px-6 py-4 dark:text-gray-200">
                    <a href="#" class="text-white bg-blue-600 hover:bg-blue-700 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm dark:bg-blue-500 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800 p-2">ダウンロード</a>
                    </td>
                </tr>
                )
            })}
            </tbody>
        </table>
    </div>
    );
}