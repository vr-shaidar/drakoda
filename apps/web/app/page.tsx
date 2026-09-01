const features = [
  ["Image", "Generate, edit, upscale and transform images through a model-aware studio."],
  ["Video", "Build asynchronous text-to-video and image-to-video workflows without blocking requests."],
  ["Audio", "Keep speech, sound and music providers behind the same extensible gateway."],
];

export default function Home() {
  return <main className="shell">
    <nav className="nav"><div className="logo">drakoda</div><div className="navlinks"><a href="#studio">Studio</a><a href="#features">Features</a><a href="#pricing">Pricing</a><a href="/api/docs">API</a></div></nav>
    <section className="hero" id="studio">
      <div className="eyebrow">AI Media Generation Platform</div>
      <h1>One studio for every creative workflow.</h1>
      <p className="lead">Generate images, video and audio with a provider-neutral creative platform designed around model capabilities, transparent credits and scalable asynchronous jobs.</p>
      <div className="actions"><a className="btn primary" href="#features">Explore Studio</a><a className="btn" href="#pricing">View pricing</a></div>
      <div className="grid" id="features">{features.map(([title, text]) => <article className="card" key={title}><h3>{title}</h3><p>{text}</p></article>)}</div>
    </section>
  </main>;
}
