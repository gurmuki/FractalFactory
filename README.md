<h1>Fractal Factory</h1>

A Windows application for generating 2D fractal images and movies. Simple inputs parametrically describe a [Newton fractal](https://en.wikipedia.org/wiki/Newton_fractal). Viewing tools support zoom in/out, window and pan within a generated image. An option supports the classic [Mandelbrot fractal](https://en.wikipedia.org/wiki/Mandelbrot_set).

<p></p>
<details>
  <summary>Example images</summary>
<img src="https://github.com/user-attachments/assets/debd45ba-543a-4ef2-b2c9-47ac664e629b" width="300" height="400">
<img src="https://github.com/user-attachments/assets/255d00ad-5864-4a73-a8e5-6d40a44b840a" width="300" height="400">
<img src="https://github.com/user-attachments/assets/5a8bf5e2-8844-4497-bb03-0244f4d084bd" width="300" height="400">
<img src="https://github.com/user-attachments/assets/b37cbf18-fbbb-4b21-9289-b8647d26a026" width="300" height="400">
<img src="https://github.com/user-attachments/assets/a0b85f68-e786-4f7a-b2c3-ef095682e6ee" width="300" height="400">
<img src="https://github.com/user-attachments/assets/e3cd9dfd-98c5-41ee-a1c6-332db6da75fd" width="300" height="400">
<img src="https://github.com/user-attachments/assets/217497fd-1987-4e84-a308-5cd38b4541c0" width="300" height="400">
<img src="https://github.com/user-attachments/assets/1744b2c6-5fe9-435a-b2e1-21720e62adcf" width="300" height="400">
<img src="https://github.com/user-attachments/assets/e077b9e3-9666-43b8-96d1-bbcd91ddad97" width="300" height="400">
</details>
<details>
  <summary>Example movies</summary>
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;There is no way to embed large videos here. Instead, a current set of videos <br>
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;can be found at https://www.youtube.com/@gurmuki/videos
</details>

<h2>Documentation</h2>

| Dialog | Description |
| ------ | --- |
| ![ControlsLarge](https://github.com/user-attachments/assets/192db74e-a138-443e-987b-23f35f71527e) |(1) Execution controls<li>Single Frame - enables the Generate button, allowing generation of a single image using the polynomial and domain values.</li><li>Multiple Frames - enables the Run and Stop buttons, allowing generation (and termination) of a sequence of images based on recorded statements.</li><p></p>(2) Polynomials<li>always enabled unless generating a [Mandelbrot fractal](https://en.wikipedia.org/wiki/Mandelbrot_set).</li><li>numer is the numerator of a [Newton fractal](https://en.wikipedia.org/wiki/Newton_fractal) equation.</li><li>denom is the denominator of a [Newton fractal](https://en.wikipedia.org/wiki/Newton_fractal) equation.</li><p></p>(3) Domain<li>the values over which the algorithms operate (mapped to the display area).</li><li>The values will be adjust to reflect the aspect ratio of the display area.</li><p></p>(4) Recording<li>Record creates a statement using the current polynomial and domain values.</li><li>Precision controls the formatting of a values mantissa.</li><li>Interpolate is enabled when two adjacent statements are selected</li><li>Interpolate provides automatic generation of multiple statements whose intermediate statement lie between its bounding statements.</li><li>Clear deletes the images associated with the selected statements.<li>Update updates the active statement and saves its associated image to a database.</li><p></p>(5) Statements<li>A grid control whose text (statements) represent a sequence of fractal images.<li><li>grid statements can be edited/inserted.</li>An image is associated with each statement.</li><li>A context menu is available RBM click.</li><p></p>|
