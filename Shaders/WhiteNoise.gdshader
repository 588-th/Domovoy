shader_type canvas_item;

uniform sampler2D screen_texture : hint_screen_texture;

uniform bool enable_noise = true;
uniform float noise_opacity : hint_range(0.0, 1.0) = 0.5;
uniform vec2 noise_resolution = vec2(200.0, 200.0);

void fragment() {
    vec4 texture_color = texture(screen_texture, SCREEN_UV);
    
    if (enable_noise) {
        vec2 noise_coords = floor(UV * noise_resolution) / noise_resolution;
		
        float noise_value = fract(sin(dot(noise_coords, vec2(12.9898, 78.233)) + TIME) * 43758.5453123);

        vec4 noise_color = vec4(vec3(noise_value), noise_opacity);

        COLOR = mix(texture_color, noise_color, noise_opacity);
    } else {
        COLOR = texture_color;
    }
}
