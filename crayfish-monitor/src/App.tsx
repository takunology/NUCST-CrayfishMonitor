import React from 'react';
import { BrowserRouter, Route } from 'react-router-dom';
import './App.css';
import Sidebar from './components/Sidebar';
import DashBoard from './Views/DashBoard';

function App() {
  return (
    <div className='row'>
      <div className='col-md-2'>
          <Sidebar/>
      </div>
      <div className='col-md-10'>
        <div className="card dashboard">
          <div className="card-header">
            サンプルプロット
          </div>
          <div className="card-body">
            <DashBoard/>
          </div>
        </div>
      </div>
    </div>
  );
}

export default App;
