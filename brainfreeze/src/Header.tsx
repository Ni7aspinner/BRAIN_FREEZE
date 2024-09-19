import { useState } from "react";
import About from "./pages/About";
import Game from "./pages/Game";

function Header() {
    const [currentPage, setCurrentPage] = useState('home');

    // Handler functions to switch between pages
    const goToHome = () => setCurrentPage('home');
    const goToAbout = () => setCurrentPage('about');
    const goToGame = () => setCurrentPage('game');
    return(
        <header>
            <h2>BrainFreeze</h2>
            <nav>
                <button onClick={goToHome}>Home</button>
                <button onClick={goToAbout}>About</button>
                <button onClick={goToGame}>Game</button>
            </nav>

            <main>
                {currentPage === 'about' && <About></About>}
                {currentPage === 'game' && <Game></Game>}
            </main>
        </header>
    );
}

export default Header;