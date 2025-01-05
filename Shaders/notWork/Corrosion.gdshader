shader_type spatial;
render_mode cull_disabled, specular_schlick_ggx;

uniform float fade_scale : hint_range(0.0, 1.0) = 0;
uniform vec2 uv_scale = vec2(1.0, 1.0);
uniform sampler2D base_texture: filter_nearest;
uniform sampler2D end_texture: filter_nearest;
uniform sampler2D noise_texture: filter_nearest;

void fragment() {
	vec2 uv = UV;
    uv = uv * uv_scale;
	ALBEDO = (texture(noise_texture, uv).r < fade_scale) ? texture(end_texture, uv).rgb : texture(base_texture, uv).rgb;
}