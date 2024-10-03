import { Link } from 'react-router-dom'

export default function Navbar() {
  return (
    <nav>
      <Link to="/"><button>Home</button></Link>
      <Link to="/about"><button>About</button></Link>
      <Link to="/game"><button>Game</button></Link>
      <Link to="/simon"><button>Simon</button></Link>
    </nav>
  )
}
