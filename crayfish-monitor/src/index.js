import React from 'react';
import ReactDOM from 'react-dom/client';
import SideBar from './components/Sidebar';
import './index.css';
import reportWebVitals from './reportWebVitals';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import DashBoard from './Views/DashBoard';
import Reference from './Views/Reference';
import Upload from './Views/Upload';
import Setting from './Views/Setting';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <BrowserRouter>
    <aside className=''>
      <SideBar/>
    </aside>
    <div className='mainpage bg-gray-100 dark:bg-gray-900'>
      <Routes>
        <Route path='/' element={<DashBoard/>}/>
        <Route path='/data' element={<Reference/>}/>
        <Route path='/upload' element={<Upload/>}/>
        <Route path='/setting' element={<Setting/>}/>
      </Routes>
    </div>
    </BrowserRouter>
  </React.StrictMode>
);

reportWebVitals();
