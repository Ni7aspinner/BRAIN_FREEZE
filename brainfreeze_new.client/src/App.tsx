import Header from './Header';
import Footer from './Footer';

import './App.css';
import { Route, Routes } from 'react-router-dom';
import Simon from './pages/Simon';
import Home from './pages/Home';
import NRG from './pages/NRG';


function App() {
    return (
        <div className="App">
        <Header/>
        <Routes>
            <Route path="/" element={<Home/>}></Route>
            <Route path="/Simon" element={<Simon/>}></Route>
            <Route path="/NRG" element={<NRG/>}></Route>
        </Routes>
        <Footer/>
        </div>
    );

}


export default App
