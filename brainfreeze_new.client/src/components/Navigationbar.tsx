// Header.js
import { Link } from 'react-router-dom';
import './Navigationbar.css';

function Header() {
    return (
        <header>
            <nav>
                <Link to="/home" className="home-arrow"></Link>
            </nav>
        </header>
    );
}

export default Header;
