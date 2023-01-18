import React from "react";
import { SidebarList } from "./SidebarList";

function Sidebar() {
    return (
        <nav className="flex-shrink-0 text-white bg-dark sidebar">
            <div className="position-sticky">
                <ul className="nav flex-column">
                    <li className="nav-title" onClick={() => window.location.pathname = "/"}>
                        CrayfishMonitor
                    </li>
                </ul>
                <hr></hr>
                <ul className="nav flex-column">
                    {SidebarList.map((value, key) => {
                        return (
                            <li className="nav-item" 
                                key={key} id={window.location.pathname == value.link ? "active" : ""} 
                                onClick={() => window.location.pathname = value.link}>
                                <div id="title">
                                    <i id="icon" className={value.icon}/>
                                    {value.title}
                                </div>
                            </li>
                        );
                    })}
                </ul>
            </div>
        </nav>
    );
}

export default Sidebar;