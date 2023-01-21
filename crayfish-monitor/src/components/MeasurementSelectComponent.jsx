const CrayfishList = [
    {
        isactive: true,
        label: "crayfish-A"
    },
    {
        isactive: false,
        label: "crayfish-B"
    },
    {
        isactive: true,
        label: "crayfish-C"
    }
]

export default function MeasurementSelectComponent() {
    return (
    <div class="block m-3 p-4 bg-white border border-gray-200 rounded-lg shadow-md dark:bg-gray-800 dark:border-gray-700">
        <h5 class="mb-2 text-2xl font-bold tracking-tight text-gray-900 dark:text-white">計測対象</h5>
        <table class="w-full mt-3 text-sm text-left overflow-x-auto text-gray-500 dark:text-gray-700">
            <thead class="text-xs bg-gray-100 text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-200">
                <tr>
                    <th scope="col" class="px-6 py-3">
                        ラベル名
                    </th>
                    <th scope="col" class="px-6 py-3">
                        状態
                    </th>
                </tr>
            </thead>
            <tbody>
            {CrayfishList.map((item, key) => {
                return (
                <tr className="bg-white border-b hover:bg-gray-100 dark:bg-gray-800 dark:border-gray-500 dark:hover:bg-gray-700">
                    <th scope="row" class="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white">
                    {item.label}
                    </th>
                    <td class="px-6 py-4 dark:text-gray-200">
                    {item.isactive ? <span>オンライン</span> : <span>オフライン</span>}
                    </td>
                </tr>
                )
            })}
            </tbody>
        </table>
    </div>
    )
}