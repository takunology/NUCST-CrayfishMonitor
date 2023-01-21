import React from "react";
import { SidebarList } from "./SidebarList"

export default function SideBar(){
    return (
        <div class="sidebar fixed top-0 bottom-0 lg:left-0 p-2 w-[240px] overflow-y-auto text-center border border-gray-200 rounded-lg shadow-md bg-white dark:bg-gray-800 dark:border-gray-700">
            <div class="text-gray-100 text-xl">
                <div class="p-2.5 mt-1 flex items-center">
                <i class="bi bi-app-indicator px-2 py-1 rounded-md bg-blue-600"></i>
                <h1 class="font-bold text-gray-600 dark:text-gray-200 text-[16pt] ml-3">CrayfishMonitor</h1>
                <i class="bi bi-x cursor-pointer ml-28 lg:hidden" onClick="openSidebar()"></i>
                </div>
                <div class="my-2 bg-gray-600 h-[1px]"></div>
            </div>
            {SidebarList.map((value, key) => {
                return (
                <div class="p-2.5 mt-3 flex items-center rounded-md px-4 duration-300 cursor-pointer hover:bg-blue-100 dark:hover:bg-blue-600 active:bg-blue-600 hover:text-white"
                    key={key}
                    onClick={()=>window.location.pathname = value.link}>
                    <i class={value.icon}></i>
                    <span class="text-[12pt] ml-4 text-gray-600 dark:text-gray-200">{value.title}</span>
                </div>
                )
            })}
        </div>
    );
}