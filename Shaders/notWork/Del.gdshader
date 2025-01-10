shader_type spatial;

uniform sampler2D texture_albedo : source_color, filter_nearest;
uniform vec2 uv_scale = vec2(1.0, 1.0);

void fragment() {
	vec2 uv = UV * uv_scale;
    vec4 color = texture(texture_albedo, uv);

    ALBEDO = color.rgb;
}
