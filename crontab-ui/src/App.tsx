import './App.css';
import Login from './features/login/login';
import Home from './features/home/home';
import { BrowserRouter, Route, RouterProvider, Routes, createBrowserRouter } from 'react-router-dom';
import React from 'react';

function App() {
  return (
    <React.StrictMode>
      <BrowserRouter>
        <Routes>
          <Route index element={<Home />} />
          <Route path="login" element={<Login />} />
          <Route path="home" element={<Home />} />
          <Route path="*" element={<p>There's nothing here: 404!</p>} />
          {/* <Route path="/*" element={<Home/>} /> */}
        </Routes>
      </BrowserRouter>
    </React.StrictMode>
  );
}

export default App;

