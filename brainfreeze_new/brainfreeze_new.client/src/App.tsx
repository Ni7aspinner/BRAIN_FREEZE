import Header from './Header';
import Footer from './Footer';

import './App.css';
import { Route, Routes } from 'react-router-dom';
import About from './pages/About';
import Game from './pages/Game';
import Simon from './pages/Simon';
import Home from './pages/Home';


function App() {
    return (
        <div className="App">
        <Header/>
        <Routes>
            <Route path="/" element={<Home/>}></Route>
            <Route path="/about" element={<About/>}></Route>
            <Route path="/game" element={<Game/>}></Route>
            <Route path="/simon" element={<Simon/>}></Route>
        </Routes>
        <Footer/>
        </div>
    );

}


export default App
