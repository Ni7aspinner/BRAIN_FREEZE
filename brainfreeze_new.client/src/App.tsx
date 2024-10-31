import Header from './Header';
import Footer from './Footer';

import './App.css';
import { Route, Routes } from 'react-router-dom';
import Simon from './pages/Simon';
import Home from './pages/Home';
import CardFlip from './pages/CardFlip'

function App() {
    return (
        <div className="App">
        <Header/>
        <Routes>
            <Route path="/" element={<Home/>}></Route>
            <Route path="/simon" element={<Simon/>}></Route>
            <Route path="/card flip" element={<CardFlip/>}></Route>
        </Routes>
        <Footer/>
        </div>
    );

}


export default App
