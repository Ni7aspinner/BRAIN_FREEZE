import { useState } from "react";
import About from "./pages/About";

function Header() {
    const [currentPage, setCurrentPage] = useState('home');

    // Handler functions to switch between pages
    const goToHome = () => setCurrentPage('home');
    const goToAbout = () => setCurrentPage('about');
    return(
        <header>
            <h2>BrainFreeze</h2>
            <nav>
                <button onClick={goToHome}>Home</button>
                <button onClick={goToAbout}>About</button>
            </nav>

            <main>
                {currentPage === 'about' && <About></About>}
            </main>
        </header>
    );
}

export default Header;