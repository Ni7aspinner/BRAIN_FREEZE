import { Link } from 'react-router-dom'

export default function Navbar() {
  return (
    <nav>
      <Link to="/"><button>Home</button></Link>
      <Link to="/card flip"><button>Card Flip</button></Link>
      <Link to="/Simon"><button>Simon</button></Link>
      <Link to="/NRG"><button>NGR</button></Link>
      <Link to="/Scoreboard"><button>Scoreboard</button></Link>
    </nav>
  )
}