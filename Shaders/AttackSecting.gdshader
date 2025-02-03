shader_type spatial;
render_mode unshaded;

uniform vec3 color : source_color = vec3(0.2, 0.0, 0.24);
uniform float alpha : hint_range(0.0, 1.0) = 1.0;

void fragment() {
    ALBEDO = color;
    ALPHA = alpha;
}
