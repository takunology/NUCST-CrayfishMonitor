import { MeasurementList } from "./MeasurementList"

export default function MeasurementTableComponent(){
    return (
    <table class="w-full text-sm text-left text-gray-500 dark:text-gray-700">
        <thead class="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-200">
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
            <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-500">
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
                <a href="#" class="text-white bg-blue-700 hover:bg-blue-700 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 mr-2 mb-2 dark:bg-blue-500 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800">ダウンロード</a>
                </td>
            </tr>
            )
        })}
        </tbody>
    </table>
    );
}