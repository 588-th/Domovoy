shader_type spatial;

uniform float teleport_range_x : hint_range(0.0, 1.0);
uniform float teleport_range_y : hint_range(0.0, 1.0);
uniform float teleport_range_z : hint_range(0.0, 1.0);
uniform float teleport_frequency : hint_range(0.0, 100.0);
uniform bool enable_blinking = true;
uniform float blink_frequency : hint_range(0.0, 100.0);

float hash(float p) {
    p = fract(p * 0.3183099 + 0.1);
    p *= 17.0;
    return fract(p * (p + 31.0));
}

vec3 random_offset(float seed) {
    return vec3(
        hash(seed),
        hash(seed + 1.0),
        hash(seed + 2.0)
    ) * 2.0 - 1.0;
}

void vertex() {
    float teleport_step = floor(TIME * teleport_frequency);

    vec3 offset = random_offset(teleport_step);

    offset.x *= teleport_range_x;
    offset.y *= teleport_range_y;
    offset.z *= teleport_range_z;

    MODELVIEW_MATRIX[3].xyz += offset;
}

void fragment() {
    if (enable_blinking) {
        float blink_phase = fract(TIME * blink_frequency);
        if (blink_phase < 0.5) {
            discard;
        }
    }
}
