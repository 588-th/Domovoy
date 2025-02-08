shader_type spatial;
render_mode blend_mix;

uniform sampler2D TEXTURE;
uniform float Frequency = 1.0;
uniform float MaxOpacity : hint_range(0, 1) = 1.0;
uniform float MinimumOpacity : hint_range(.5, 1) = 0.0;

void fragment() {
    vec4 tex_color = texture(TEXTURE, UV);

    float base_alpha = tex_color.a;
    float time_based_factor = sin(Frequency * TIME);

    float opacity_range = (1.0 - time_based_factor) * MinimumOpacity;
    float final_opacity = fma(time_based_factor, MaxOpacity, opacity_range);

    ALBEDO = tex_color.rgb;
    ALPHA = base_alpha * final_opacity;
}
